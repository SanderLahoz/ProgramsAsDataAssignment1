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

// Formatting arithmetic expressions
let rec fmt ae : string =
    match ae with
    | aexpr.CstI i -> (string i)
    | Var x -> x
    | Add(ae1, ae2) -> "(" + (fmt ae1) + "+" + (fmt ae2) + ")"
    | Mul(ae1, ae2) -> "(" + (fmt ae1) + "*" + (fmt ae2) + ")"
    | Sub(ae1, ae2) -> "(" + (fmt ae1) + "-" + (fmt ae2) + ")"

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
    | If(e1, e2, e3) ->
        if (eval' e1 env) > 0 then
            (eval' e2 env)
        else
            (eval' e3 env)

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
