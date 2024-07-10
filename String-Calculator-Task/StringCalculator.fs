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
            let number = parseString (sb.ToString())
            if number <= 1000 then
                result.Add(number)

    let separateDelimitersAndInput (input: string) : Set<char> * string =
        if not (input.StartsWith("\\")) then
            (set [','; '\n'], input)
        else
            let endOfDelimiters = input.IndexOf('\n')
            if endOfDelimiters = -1 then
                raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Delimiter section not properly terminated"))
            let delimiters = 
                input.Substring(1, endOfDelimiters - 1)
                |> Seq.map (fun c -> c)
                |> set
            (delimiters, input.Substring(endOfDelimiters + 1))


    let validateNegatives (result: ResizeArray<int>) =
        let negativeNumbers = result |> Seq.filter (fun number -> number < 0) |> List.ofSeq
        if negativeNumbers.Length > 0 then
            let message = String.Join(", ", negativeNumbers)
            raise (InvalidInputException(ErrorMessages.INVALID_INPUT + "Negatives not allowed: " + message))


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
