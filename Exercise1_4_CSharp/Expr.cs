namespace DefaultNamespace;

abstract class Expr
{
    public abstract override string ToString();

    public abstract int Eval(List<(string, int)> env);
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

    public override int Eval(List<(string, int)> env)
    {
        return i;
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

    public override int Eval(List<(string, int)> env)
    {
        foreach (var (name, v) in env)
        {
            if (name == x)
                return v;
        }
        throw new Exception(x + " not found");
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

    protected abstract int Combine(int v1, int v2);

    public override int Eval(List<(string, int)> env)
    {
        int v1 = E1.Eval(env);
        int v2 = E2.Eval(env);
        return Combine(v1, v2);
    }
}

class Add : Binop
{
    public Add(Expr e1, Expr e2) : base(e1, e2){}
        
    protected override string Symbol()
    {
        return "+";
    }

    protected override int Combine(int v1, int v2)
    {
        return v1 + v2;
    }
}

class Sub : Binop
{
    public Sub(Expr e1, Expr e2) : base(e1, e2){}
        
    protected override string Symbol()
    {
        return "-";
    }
    
    protected override int Combine(int v1, int v2)
    {
        return v1 - v2;
    }
}

class Mul : Binop
{
    public Mul(Expr e1, Expr e2) : base(e1, e2){}
        
    protected override string Symbol()
    {
        return "*";
    }
    
    protected override int Combine(int v1, int v2)
    {
        return v1 * v2;
    }
}