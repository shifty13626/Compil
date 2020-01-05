function pow(a, n)
{
    var i;
    var result = a;
    for (i = n-1; i > 0; i = i - 1)
    {
        result = result * a;
    }
    return result;
}