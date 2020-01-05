#include<stdlib>
#include<stdio>

#define LIMIT 16

function test(a, b)
{
    var i;
    for (i = 0; i < 5; i = i + 1) {
        print(i);
        var j;
            for (j = 0; j < 3; j = j + 1) {
                print(j);
                break;
            }
    }


    return a + b;
}

function main()
{
    var i = 2 ^ 3;
    i = test(i, 2);
    print(i);
    return 0;
}