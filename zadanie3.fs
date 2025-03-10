﻿open System

type Complex = { Re : float; Im : float }

let parseComplex (s : string) : Complex option =
    try
        let parts = s.Split('+')
        if parts.Length <> 2 then
            None
        else
            let rePart = parts.[0].Trim()
            let imPart = parts.[1].Replace("i", "").Trim()

            let re = float rePart
            let im = float imPart

            Some { Re = re; Im = im }
    with
        | _ -> None

let addComplex (z1 : Complex) (z2 : Complex) : Complex =
    { Re = z1.Re + z2.Re; Im = z1.Im + z2.Im }

let subtractComplex (z1 : Complex) (z2 : Complex) : Complex =
    { Re = z1.Re - z2.Re; Im = z1.Im - z2.Im }

let multiplyComplex (z1 : Complex) (z2 : Complex) : Complex =
    { Re = z1.Re * z2.Re - z1.Im * z2.Im; Im = z1.Re * z2.Im + z1.Im * z2.Re }

let divideComplex (z1 : Complex) (z2 : Complex) : Complex =
    let denominator = z2.Re * z2.Re + z2.Im * z2.Im
    if denominator = 0.0 then
        failwith "Деление на ноль"
    else
        { Re = (z1.Re * z2.Re + z1.Im * z2.Im) / denominator; Im = (z1.Im * z2.Re - z1.Re * z2.Im) / denominator }

let rec powerComplex (z : Complex) (n : int) : Complex =
    if n < 0 then
        failwith "Степень должна быть неотрицательной"
    elif n = 0 then
        { Re = 1.0; Im = 0.0 }
    elif n = 1 then
        z
    else
        multiplyComplex z (powerComplex z (n - 1))

let printComplex (z : Complex) : string =
    sprintf "%f + %fi" z.Re z.Im

[<EntryPoint>]
let main argv =
    printfn "Введите первое комплексное число (в формате a+bi):"
    let input1 = Console.ReadLine()

    printfn "Введите второе комплексное число (в формате a+bi):"
    let input2 = Console.ReadLine()

    match parseComplex input1, parseComplex input2 with
    | Some z1, Some z2 ->
        printfn "Выберите операцию (+, -, *, /, ^):"
        let operation = Console.ReadLine()

        match operation with
        | "+" -> printfn "Результат: %s" (printComplex (addComplex z1 z2))
        | "-" -> printfn "Результат: %s" (printComplex (subtractComplex z1 z2))
        | "*" -> printfn "Результат: %s" (printComplex (multiplyComplex z1 z2))
        | "/" ->
            try
                printfn "Результат: %s" (printComplex (divideComplex z1 z2))
            with
            | Failure msg -> printfn "%s" msg
        | "^" ->
            printfn "Введите степень (целое число):"
            let powerInput = Console.ReadLine()
            match System.Int32.TryParse(powerInput) with
            | true, power ->
                try
                    printfn "Результат: %s" (printComplex (powerComplex z1 power))
                with
                | Failure msg -> printfn "%s" msg
            | false, _ -> printfn "Неверный формат степени.  Пожалуйста, введите целое число."
        | _ -> printfn "Неверная операция."

    | None, _ -> printfn "Неверный формат первого комплексного числа."
    | _, None -> printfn "Неверный формат второго комплексного числа."

    0