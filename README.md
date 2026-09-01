# ProgramsAsDataAssignment1

##Overview
Ex 1.1 and 1.2 are in the Intro2.fs file in the Intro folder.
Ex 1.4 is located in the Expr.cs in Exercise1_4_CSharp folder and was written in C#.
Ex 2.1, 2.2, and 2.3 are in the Intcomp1.fs file in hte Intcomp folder.

##Note
To make the code easily distinguishable, every line or section that was written or modified by us was
annotated with "Written by us" comments (or similar, e.g. TODO notes).


##Exercise 1.1 - in Intro2.fs
i) We extended the expr type with a new If(expr, expr, expr) constructor, and extended eval 
to handle three additional operators: "max", "min", and "==" that return 1 for true, and 0 for false).
ii) Added example expressions e4 to e7 using the new operators that are evaluated using eval/eval'.
iii) Added eval' that evaluates both arguments of a Prim (i1, i2) before branching on teh operator
string.
iv) Extended eval to handle the new If(e1, e2, e3) case which evaluates e1, and e2 if the result is
greater than 0, otherwise evaluates e3.


##Exercise 1.2 - in Intro2.fs
i) Declared aexpr without let-bindings, with constructors CstrI, Var, Add, Sub, and Mul.
ii) Added example expressions e8 to e10 that represent v - (w + z), 2 * (v- (w + z)), and 
x + y + z + v.
iii) Implemented fmt: aexpr -> string to format aexpr values as strings. The binary operations 
are wrapped in parentheses.
iv) Implemented simplify: aexpr -> aexpr, that simplifies both sides of the binary operation 
before it checks whether either of teh sides can be eliminated or folded (like adding zero, 
multiplying by one, etc.).
v) Implemented differentiate: aexpr -> string _> aexpr, which does differentiation by computing
the symbolic dervative of an arithmetic exoression.

##Exercise 1.4 - in Expr.cs and Program.cs
There was an option in the exercise between choosing Java and C#. For this exercise, 
we are using C#.
i) We built the classes that followed the aexpr type: an abstract Expr base class, CstI and Var
as leaf classes, and an abstract Binop class with three subclasses: Add, Sub, and Mul.
ii) Added three more example expressions to the Program.cs file.
iii) Added Eval(List<(string, int)> env) in the needed classes/subclasses (mirroring the
(string* int) list environment in F#). The Binop implementation is built on the abstract
Combine(int, int) method.
iv) Added Simplify() to Add, Sub, and Mul, implementing the same identity/zero rules and 
constant folding as the simplify in the 1.2 iv. CstI and Var simplify to themselves.


##Exercise 2.1 - in Intcomp1.fs
We extended the expr type so taht Let takes a list of string * expr instead of 
just one, which allows multiple let-bindings that are sequential
in one let expression. Also, revised Let in eval with a evalEnv helper that folds over a list of
bindings and evaluates each right-hand side within the environment built up by all of the 
previous bindings in the same let, and then evaluates the body in the final environment.


##Exercise 2.2
Revised Let in the freevars with aux helper that accumulates the name bound while walking the 
bindings. For each right-hand side of the bidnings, only the variables that are bound by the
earlier bindings in the same let are excluded, not the variable that is currently being 
bound, and also not the later bindings.


##Exercise 2.3
Revised Ler in the tcomp with comp helper that folds over the bindings, produces TLet 
expressions, and extends the compile time environment cenv with each of the bound
variable name as it goes. We did not make any chnages to texpr or teval.