function printx(x)
{
    if(x != 0)
    {
        printx(x / 10);
        var b;
        b = x % 10 + 48;
        send b;
    }
}

function print(y)
{
    if(y == 0)
    {
        send 48;
    }
    else
    {
        if(y < 0)
        {
            send 45;
            printx(-y);
        }
        else
        {
            printx(y);
        }
    }
    send 10;
}
