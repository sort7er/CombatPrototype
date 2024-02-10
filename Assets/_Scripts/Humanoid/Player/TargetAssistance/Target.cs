public class Target
{
    public Enemy enemy;
    public float dotProduct;
    public float distance;

    public Target(Enemy enemy, float dotProduct, float distance)
    {
        this.enemy = enemy;
        this.dotProduct = dotProduct;
        this.distance = distance;
    }
}