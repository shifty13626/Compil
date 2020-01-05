#include<stdlib>
#include<stdio>

function main()
{
	var i;
	var j;

	for (i = 0; i < 5; i = i + 1)
	{
		print(i);
		for (j = 10; j > 7; j = j - 1)
		{
			print(j);
		}
	}

	return 0;
}