#include<stdlib>
#include<stdio>

function test(a, b)
{
    return a + b;
}

function main()
{
    var i = 2 ^ 3;
    i = test(i, 2);
    print(i);
    return 0;
}