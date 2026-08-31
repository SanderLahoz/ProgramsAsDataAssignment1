using DefaultNamespace;

Expr e = new Add(new CstI(17), new Var("z"));
Console.WriteLine(e.ToString());

Expr e1 = new Sub(new CstI(35), new CstI(5));
Console.WriteLine(e1.ToString());

Expr e2 = new Mul(new CstI(2), new Var("a"));
Console.WriteLine(e2.ToString());

Expr e3 = new Mul(new CstI(10), new CstI(2));
Console.WriteLine(e3.ToString());

var env = new List<(string, int)> { ("b", 5) };
Expr e4 = new Add(new CstI(2), new Var("b"));
Console.WriteLine(e4.Eval(env));

Expr toSimplify = new Add(new Var("c"), new CstI(0));
Console.WriteLine(toSimplify.Simplify());

Expr toSimplify2 = new Mul(new CstI(1), new Mul(new Var("d"), new CstI(0)));
Console.WriteLine(toSimplify2.Simplify());