{
    {
        var r = 0;
        var b = 0;
        var i;
        for (i = 0; i <=1000; i = i + 1){
            var j = 0;
            while ( j < i ) {
                b = b + i + j;
                j = j + 100;
            }
        }
        r = b;
    }
}