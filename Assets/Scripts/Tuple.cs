public class Tuple<A, B>
{
    public A first;
    public B second;
    public Tuple(A first, B second)
    {
        this.first = first;
        this.second = second;
    }
}

public class Tuple<A, B, C>
{
    public A first;
    public B second;
    public C third;
    public Tuple(A first, B second, C third)
    {
        this.first = first;
        this.second = second;
        this.third = third;
    }
}
