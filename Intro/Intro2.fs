(* Programming language concepts for software developers, 2010-08-28 *)

(* Evaluating simple expressions with variables *)

module Intro2

(* Association lists map object language variables to their values *)

let env = [ ("a", 3); ("c", 78); ("baf", 666); ("b", 111) ]

let emptyenv = [] (* the empty environment *)

let rec lookup env x =
    match env with
    | [] -> failwith (x + " not found")
    | (y, v) :: r -> if x = y then v else lookup r x

let cvalue = lookup env "c"


(* Object language expressions with variables *)

type expr =
    | CstI of int
    | Var of string
    | Prim of string * expr * expr
    | If of expr * expr * expr

type aexpr =
    | CstI of int
    | Var of string
    | Add of aexpr * aexpr
    | Mul of aexpr * aexpr
    | Sub of aexpr * aexpr


(* Evaluation within an environment *)

let rec eval (e: expr) (env: (string * int) list) : int =
    match e with
    | expr.CstI i -> i
    | expr.Var x -> lookup env x
    | Prim("+", e1, e2) -> eval e1 env + eval e2 env
    | Prim("*", e1, e2) -> eval e1 env * eval e2 env
    | Prim("-", e1, e2) -> eval e1 env - eval e2 env
    | Prim("min", e1, e2) ->
        if (eval e1 env) < (eval e2 env) then
            eval e1 env
        else
            eval e2 env
    | Prim("max", e1, e2) ->
        if (eval e1 env) > (eval e2 env) then
            eval e1 env
        else
            eval e2 env
    | Prim("==", e1, e2) -> if (eval e1 env) = (eval e2 env) then 1 else 0
    | Prim(_, _, _) -> failwith "Unexpected pattern for Prim in expression"
    | If(_, _, _) -> failwith "Unexpected pattern for If in expression"

let rec eval' e (env: (string * int) list) : int =
    match e with
    | expr.CstI i -> i
    | expr.Var x -> lookup env x
    | Prim(ope, e1, e2) ->
        let i1 = eval' e1 env
        let i2 = eval' e2 env

        match ope with
        | "+" -> i1 + i2
        | "*" -> i1 * i2
        | "-" -> i1 - i2
        | "min" -> if i1 < i2 then i1 else i2
        | "max" -> if i1 > i2 then i1 else i2
        | "==" -> if i1 = i2 then 1 else 0
        | _ -> failwith "unexpected operator"

    | If(e1, e2, e3) ->
        if (eval' e1 env) > 0 then
            (eval' e2 env)
        else
            (eval' e3 env)


// Formatting arithmetic expressions
let rec fmt ae : string =
    match ae with
    | aexpr.CstI i -> (string i)
    | Var x -> x
    | Add(ae1, ae2) -> "(" + (fmt ae1) + " + " + (fmt ae2) + ")"
    | Mul(ae1, ae2) -> "(" + (fmt ae1) + " * " + (fmt ae2) + ")"
    | Sub(ae1, ae2) -> "(" + (fmt ae1) + " - " + (fmt ae2) + ")"

let rec simplify ae : aexpr =
    match ae with
    | CstI _ -> ae
    | Var _ -> ae
    | Add(ae1, ae2) ->
        let ae1' = simplify ae1
        let ae2' = simplify ae2

        match ae1', ae2' with
        | Var x, CstI 0 -> Var x
        | CstI 0, Var x -> Var x
        | CstI 0, CstI x -> CstI x
        | CstI x, CstI 0 -> CstI x
        | CstI x, CstI y -> CstI(x + y)
        | x, y -> Add(simplify x, simplify y)
    | Mul(ae1, ae2) ->
        let ae1' = simplify ae1
        let ae2' = simplify ae2

        match ae1', ae2' with
        | Var _, CstI 0 -> CstI 0
        | CstI 0, Var x -> CstI 0
        | CstI 0, CstI x -> CstI 0
        | CstI _, CstI 0 -> CstI 0
        | Var x, CstI 1 -> Var x
        | CstI 1, Var x -> Var x
        | CstI 1, CstI x -> CstI x
        | CstI x, CstI 1 -> CstI x
        | CstI x, CstI y -> CstI(x * y)
        | x, y -> Mul(simplify x, simplify y)
    | Sub(ae1, ae2) ->
        let ae1' = simplify ae1
        let ae2' = simplify ae2

        match ae1', ae2' with
        | Var x, CstI 0 -> Var x
        | CstI x, CstI 0 -> CstI x
        | CstI x, CstI y when x = y -> CstI 0
        | CstI x, CstI y -> CstI(x - y)
        | x, y -> Sub(simplify x, simplify y)

let rec differentiate ae v =
    match ae with
    | CstI _ -> CstI 0
    | Var x when x = v -> CstI 1
    | Var _ -> CstI 0
    | Add(ae1, ae2) -> Add(differentiate ae1 v, differentiate ae2 v)
    | Sub(ae1, ae2) -> Sub(differentiate ae1 v, differentiate ae2 v)
    | Mul(ae1, ae2) -> Add(Mul(differentiate ae1 v, ae2), Mul(ae1, differentiate ae2 v))




// Examples and testing

let e1: expr = expr.CstI 17

let e2 = Prim("+", expr.CstI 3, expr.Var "a")

let e3 = Prim("+", Prim("*", expr.Var "b", expr.CstI 9), expr.Var "a")

// Should evaluate to false (5 + 7) == 10 ~> false
let e4 = Prim("==", Prim("+", expr.CstI 5, expr.CstI 7), expr.CstI 10)

// Should evaluate to true ("a" + 7) ~> (3 + 7) == 10 ~> true
let e5 = Prim("==", Prim("+", expr.Var "a", expr.CstI 7), expr.CstI 10)
let e6 = Prim("min", expr.CstI 5, expr.CstI 6)
let e7 = Prim("max", expr.CstI 5, expr.CstI 6)

// Arithmetic expressions
let e8 = Sub(Var "v", Add(Var "w", Var "z"))
let e9 = Mul(CstI 2, Sub(Var "v", Add(Var "w", Var "z")))
let e10 = Add(Add(Add(Var "x", Var "y"), Var "z"), Var "v")

let e11 = Mul(Add(CstI 1, CstI 0), Add(Var "x", CstI 0))


let e1v = eval e1 env
let e2v1 = eval e2 env
let e2v2 = eval e2 [ ("a", 314) ]
let e3v = eval e3 env
let e4v = eval e4 env
let e5v = eval e5 env
let e6v = eval e6 env
let e7v = eval e7 env


let e1v' = eval' e1 env
let e2v1' = eval' e2 env
let e2v2' = eval' e2 [ ("a", 314) ]
let e3v' = eval' e3 env
let e4v' = eval' e4 env
let e5v' = eval' e5 env
let e6v' = eval' e6 env
let e7v' = eval' e7 env

let e8v1 = fmt e8
let e9v1 = fmt e9
let e10v1 = fmt e10

let e11v1 = simplify e11
