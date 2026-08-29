namespace DefaultNamespace;

abstract class Expr
{
    public abstract override string ToString();
}

class CstI : Expr
{
    public readonly int i;

    public CstI(int i)
    {
        this.i = i;
    }

    public override string ToString()
    {
        return i.ToString();
    }
}

class Var : Expr
{
    public readonly string x;

    public Var(string x)
    {
        this.x = x;
    }
    
    public override string ToString()
    {
        return x;
    }
}

abstract class Binop : Expr
{
    protected abstract string Symbol();
    
    public readonly Expr E1;
    public readonly Expr E2;

    public Binop(Expr E1, Expr E2)
    {
        this.E1 = E1;
        this.E2 = E2;
    }
    
    public override string ToString()
    {
        return "(" + E1.ToString() + Symbol() + E2.ToString() + ")";
    }
}

class Add : Binop
{
    public Add(Expr e1, Expr e2) : base(e1, e2){}
        
    protected override string Symbol()
    {
        return "+";
    }
}

class Sub : Binop
{
    public Sub(Expr e1, Expr e2) : base(e1, e2){}
        
    protected override string Symbol()
    {
        return "-";
    }
}

class Mul : Binop
{
    public Mul(Expr e1, Expr e2) : base(e1, e2){}
        
    protected override string Symbol()
    {
        return "*";
    }
}