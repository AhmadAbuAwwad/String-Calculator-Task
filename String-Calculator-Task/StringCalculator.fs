module StringCalculator

open System
open System.Collections.Generic
open System.Text

type InvalidInputException(message: string) =
    inherit Exception(message)

module ErrorMessages =
    let INVALID_INPUT = "Invalid input: "

module StringUtils =

    let parseString (str: string) =
        match Int32.TryParse(str) with
        | true, value -> value
        | false, _ -> raise (InvalidInputException(ErrorMessages.INVALID_INPUT + str + " is not a number"))

    let addNumberStringToList (result: ResizeArray<int>) (sb: System.Text.StringBuilder) =
        if sb.Length > 0 then
            let number = parseString (sb.ToString())
            if number <= 1000 then
                result.Add(number)

    let parseMultipleDelimiters (delimiterPart: string) (delimiters: ResizeArray<char>) =
        let stack = Stack<char>()
        let currentDelimiter = StringBuilder()
        for ch in delimiterPart.ToCharArray() do
            match ch with
            | '[' ->
                if stack.Count > 0 then
                    raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Nested delimiters are not allowed"))
                stack.Push(ch)
            | ']' ->
                if stack.Count = 0 || stack.Pop() <> '[' then
                    raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Unmatched delimiter brackets"))
                for d in currentDelimiter.ToString().ToCharArray() do
                    delimiters.Add(d)
                currentDelimiter.Clear() |> ignore
            | _ ->
                if not (stack.Count <= 0) then
                    currentDelimiter.Append(ch) |> ignore

    let separateDelimitersAndInput (input: string) =
        if not (input.StartsWith("\\")) then
            (set [','; '\n'], input)
        else
            try
                let endOfDelimiters = input.IndexOf('\n')
                if endOfDelimiters = -1 then
                    raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Delimiter section not properly terminated"))

                let delimiterPart = input.Substring(1, endOfDelimiters - 1)
                let input' = input.Substring(endOfDelimiters + 1)

                if delimiterPart.Contains("[") then
                    let delimiters = ResizeArray<char>()
                    parseMultipleDelimiters delimiterPart delimiters
                    (Set.ofList (delimiters |> List.ofSeq), input')
                else
                    (delimiterPart.ToCharArray() |> Seq.toList |> Set.ofList, input')
             with
            | :? System.Exception ->
            raise (InvalidInputException("Invalid input: Invalid delimiter format"))

    let validateNegatives (result: ResizeArray<int>) =
        let negativeNumbers = result |> Seq.filter (fun number -> number < 0) |> List.ofSeq
        if negativeNumbers.Length > 0 then
            let message = String.Join(", ", negativeNumbers)
            raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Negatives not allowed: " + message))


    let splitString (input: string) (delimiters: Set<char>) =
        let result = ResizeArray<int>()
        let sb = System.Text.StringBuilder()
       
        for ch in input do
            if delimiters.Contains(ch) then
                addNumberStringToList result sb
                sb.Clear() |> ignore
            else
                sb.Append(ch) |> ignore

        addNumberStringToList result sb
        validateNegatives result
        result |> List.ofSeq

// let validateInput (numArray: int list) =
//    if numArray.Length > 2 then
//      raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Number of inputs exceeds 2"))


let add (numbers: string) =
    if String.IsNullOrEmpty(numbers) then 0
    else
        let delimiters, input = StringUtils.separateDelimitersAndInput numbers
        let numList = StringUtils.splitString input delimiters
//        validateInput numList
        numList |> List.sum
