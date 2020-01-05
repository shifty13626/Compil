#include<stdlib>
#include<stdio>

#define LIMIT 15

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
<<<<<<< HEAD
	var i;
	do
	{
		i = i + 1;
		print(i);
	} while (i < 5);

	return 0;
=======
    var i = 2 ^ 3;
    i = test(i, 2);
    print(i);
    return 0;
>>>>>>> declaration_opti
}