namespace DefaultNamespace;

abstract class Expr
{
    public abstract override string ToString();

    public abstract int Eval(List<(string, int)> env);

    public abstract Expr Simplify();
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

    public override Expr Simplify()
    {
        return this;
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
    
    public override Expr Simplify()
    {
        return this;
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

    public override Expr Simplify()
    {
        Expr sime1 = E1.Simplify();
        Expr sime2 = E2.Simplify();

        if (sime1 is CstI cone1 && sime2 is CstI cone2)
            return new CstI(cone1.i + cone2.i);
        
        //0 + e -> e
        if (sime1 is CstI check1 && check1.i == 0)
            return sime2;
        
        //e + 0 -> e
        if (sime2 is CstI check2 && check2.i == 0)
            return sime1;

        return new Add(sime1, sime2);
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
    
    public override Expr Simplify()
    {
        Expr sime1 = E1.Simplify();
        Expr sime2 = E2.Simplify();

        if (sime1 is CstI cone1 && sime2 is CstI cone2)
            return new CstI(cone1.i - cone2.i);
        
        //e - 0 -> e
        if (sime2 is CstI check1 && check1.i == 0)
            return sime1;
        
        //e - e -> 0
        if (sime1.ToString() == sime2.ToString())
            return new CstI(0);

        return new Sub(sime1, sime2);
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
    
    public override Expr Simplify()
    {
        Expr sime1 = E1.Simplify();
        Expr sime2 = E2.Simplify();

        if (sime1 is CstI cone1 && sime2 is CstI cone2)
            return new CstI(cone1.i * cone2.i);
        
        //1 * e -> e
        if (sime1 is CstI check1 && check1.i == 1)
            return sime2;
        
        //e * 1 -> e
        if (sime2 is CstI check2 && check2.i == 1)
            return sime1;
        
        //0 * e -> 0
        if (sime1 is CstI check3 && check3.i == 0)
            return new CstI(0);
        
        //e * 0 -> 0
        if (sime2 is CstI check4 && check4.i == 0)
            return new CstI(0);

        return new Mul(sime1, sime2);
    }
}