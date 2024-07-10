module StringCalculator

open System

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
            result.Add(parseString (sb.ToString()))

    let splitString (input: string) (delimiters: Set<char>) =
        let result = ResizeArray<int>()
        let sb = System.Text.StringBuilder()
        let mutable lastWasDelimiter = false

        for ch in input do
            if delimiters.Contains(ch) then
                if lastWasDelimiter then
                    raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Consecutive delimiters found"))
                lastWasDelimiter <- true
                addNumberStringToList result sb
                sb.Clear() |> ignore
            else
                lastWasDelimiter <- false
                sb.Append(ch) |> ignore

        addNumberStringToList result sb
        result |> List.ofSeq

// let validateInput (numArray: int list) =
//    if numArray.Length > 2 then
//      raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Number of inputs exceeds 2"))


let add (numbers: string) =
    if String.IsNullOrEmpty(numbers) then 0
    else
        let delimiters = set [','; '\n']
        let numList = StringUtils.splitString numbers delimiters
//        validateInput numList
        numList |> List.sum
