module StringCalculatorTest

open NUnit.Framework
open StringCalculator
open StringCalculator.ErrorMessages

[<SetUp>]
let setUp () = ()

[<Test>]
let ``testAdd_EmptyString_ReturnsZero`` () =
    Assert.AreEqual(0, add "")

[<Test>]
let ``testAdd_SingleNumber_ReturnsNumber`` () =
    Assert.AreEqual(5, add "5")

[<Test>]
let ``testAdd_TwoNumbers_ReturnsSum`` () =
    Assert.AreEqual(300, add "100,200")

[<Test>]
let ``testAdd_ManyNumbers_ReturnsSum`` () =
    Assert.AreEqual(15, add "1,2,3,4,5")

//[<Test>]
//let ``testAdd_MoreThanTwoNumbers_ThrowsInvalidInputException`` () =
//    let ex = Assert.Throws<InvalidInputException>(fun () -> add "1,2,3" |> ignore)
//    Assert.AreEqual(INVALID_INPUT + "Number of inputs exceeds 2", ex.Message)

[<Test>]
let ``testAdd_ConsecutiveDelimiters_ThrowsInvalidInputException`` () =
    let ex = Assert.Throws<InvalidInputException>(fun () -> add "1,\n2" |> ignore)
    Assert.AreEqual(INVALID_INPUT + "Consecutive delimiters found", ex.Message)

[<Test>]
let ``testAdd_NonNumericCharacter_ThrowsInvalidInputException`` () =
    let ex = Assert.Throws<InvalidInputException>(fun () -> add "1\n?" |> ignore)
    Assert.AreEqual(INVALID_INPUT + "? is not a number", ex.Message)

[<Test>]
let ``testAdd_NewLineDelimiter_ReturnsSum`` () =
    Assert.AreEqual(15, add "1\n2\n3\n4\n5")

    
[<Test>]
let ``testAdd_CustomDelimiter_ReturnsSum`` () =
    Assert.AreEqual(6, add "\\;\n1;2;3")

[<Test>]
let ``testAdd_ConsecutiveCustomDelimiters_ThrowsInvalidInputException`` () =
    let ex = Assert.Throws<InvalidInputException>(fun () -> add "\\;\n1;;n2" |> ignore)
    Assert.AreEqual(INVALID_INPUT + "Consecutive delimiters found", ex.Message)

