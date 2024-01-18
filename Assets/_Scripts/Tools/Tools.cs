public static class Tools
{
    public static float Remap(float value, float from1 = 0, float to1 = 60, float from2 = 0, float to2 = 1)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }
}
