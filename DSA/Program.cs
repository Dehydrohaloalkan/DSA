using System.Text;
using DSA;

var str = "BSUIR";
var data = Encoding.ASCII.GetBytes(str);


var sc = new SignatureCreator(593, 3559, 3, 17);

var newData = sc.Create(data, 23).data;

Console.WriteLine(Encoding.ASCII.GetString(newData));

var (v, r, s) = sc.Check(newData);

Console.WriteLine($"v : {v}, r : {r}");