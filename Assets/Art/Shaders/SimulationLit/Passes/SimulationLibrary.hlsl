#ifndef SIMULATIONLIB_INCLUDED
#define SIMULATIONLIB_INCLUDED

//SIM SHADER
uniform float _Transition; //how big the lerp between source and pattern, and pattern and colortop is
uniform float _TriplanarBlend; //blend the edges to make triplanar not look jarring
uniform float _Tiling; //tiling of the pattern
uniform float _PatternSpace; //how much space the pattern gets before the transition starts
uniform float4 _PatternEmissionColor; //emission color
uniform float4 _PatternEmissionColorTop; //emission color 2
uniform sampler2D _Pattern; //the pattern texture
uniform float4 _Pattern_ST; //the pattern texture scale and offset

uniform float4 _ColorTop; //flat color which represents the unloaded simulation
uniform float3 _Origin; //The player spawn pos
uniform float _Radius; //Initial radius to start the simulation

uniform float4 _Spheres[10]; //Array for spheres of influcence (x,y,z,radius)
uniform int _SphereAmount; //amount of spheres

#pragma multi_compile __ ENABLE_SIMULATION
#pragma multi_compile __ SINGLE_EMISSION
#pragma multi_compile __ STAGE_ENTER

#define TAU 6.283185307179586 //2*PI

float InverseLerp(float a, float b, float v) {
    return (v-a)/(b-a);
}

float4 SimAlgorithm(float4 color, float3 worldPos, float3 worldNormal) {
    #ifndef ENABLE_SIMULATION
    return color;
    #else
    //Triplanar pattern
    float3 coords = worldPos * _Tiling;
    float3 blend = pow(abs(worldNormal.xyz), _TriplanarBlend);
    blend /= dot(blend, 1.0);
    const float4 patternX = tex2D(_Pattern, coords.zy);
    const float4 patternY = tex2D(_Pattern, coords.xz);
    const float4 patternZ = tex2D(_Pattern, coords.xy);
    float4 patternResult = patternX * blend.x + patternY * blend.y + patternZ * blend.z;

    #ifdef STAGE_ENTER
    //Calculate transition thresholds
    const float transitionStartBottom = _Radius - _PatternSpace;
    const float transitionEndBottom = transitionStartBottom - _Transition;
    const float transitionStartTop = _Radius + _PatternSpace;
    const float transitionEndTop = transitionStartTop + _Transition;
    
    //Calculate distance from origin
    const float distance = length(worldPos - _Origin);

    //Incorporate emission into the pattern
    #ifdef SINGLE_EMISSION
    patternResult *= _PatternEmissionColor;
    #else
    const float emissionTransition = saturate(InverseLerp(transitionStartBottom, transitionStartTop, distance));
    const float4 emissionColor = lerp(_PatternEmissionColor, _PatternEmissionColorTop, emissionTransition);
    patternResult *= emissionColor;
    #endif

    //Determine transition based on distance and lerp between the pattern and color
    float transition;
    float4 lerpResult;
    if (distance < transitionStartBottom) {
        transition = saturate(InverseLerp(transitionEndBottom, transitionStartBottom, distance));
        lerpResult = lerp(color, patternResult, transition);
    } else if (distance > transitionStartTop) {
        transition = saturate(InverseLerp(transitionStartTop, transitionEndTop, distance));
        lerpResult = lerp(patternResult, _ColorTop, transition);
    } else {
        //Pure pattern
        lerpResult = patternResult;
    }

    return lerpResult;
    
    #else //Normal look, potentially having spheres of influence
    
    //Calculate normalized transition factor and accumulate
    float accumulatedTransition = 1.0;
    for (int i = 0; i < _SphereAmount; ++i) {
        const float3 sphereOrigin = _Spheres[i].xyz;
        const float sphereRadius = _Spheres[i].w;

        //Calculate distance from the sphere's origin
        float distance = length(worldPos - sphereOrigin);

        //Determine transition thresholds
        const float transitionStart = sphereRadius;
        const float transitionEnd = transitionStart - _Transition;

        //Calculate normalized transition factor
        if (distance < transitionStart) {
            const float transitionFactor = saturate(InverseLerp(transitionStart, transitionEnd, distance));
            accumulatedTransition = saturate(accumulatedTransition - transitionFactor); //Make sure final transition is clamped between 0 and 1
        }
    }

    //Blend between pattern and color using the accumulated normalized transition factor
    patternResult *= _PatternEmissionColor;
    float4 lerpResult = lerp(patternResult, color, accumulatedTransition);
    return lerpResult;
    
    #endif
    #endif
}

void SimAlgorithm_float(float4 color, float3 worldPos, float3 worldNormal, out float4 outColor) {
    outColor = SimAlgorithm(color, worldPos, worldNormal);
}

#endif