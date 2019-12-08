module WordGuesser.GameManager

open WordGuesser.Config
open WordGuesser.GameLogic
open System

// 0.2. Repeat step 2-4 from module GameLogic until word has been guessed
let rec runGame (correctAnswer : char list, guessedValues : char list) =
    Console.Clear()
    printfn "Welcome to this fun game, try to guess the correct word"
    printfn "You tried following guesses %A" guessedValues
    let hiddenAnswer = writePartialWord (correctAnswer, guessedValues)
    if correctAnswer.Equals(hiddenAnswer) then printfn "CONGRATULATION You used %A tries!" (guessedValues.Length) else 
        let str = String.Concat(Array.ofList(hiddenAnswer))
        printfn "%A" str
        printfn "For Help press key 1" 
        let keyPressed =  getKeyPressed (guessedValues, correctAnswer)
        let guessedValues = keyPressed :: guessedValues
        let guess = isGuessCorrect (keyPressed, correctAnswer)
        printfn "%A" guess
        runGame(correctAnswer, guessedValues) 

// 0.1. Starting the game
let startGame =
    let hiddenWord = getRandomWord 
    let correctAnswer = Seq.toList (hiddenWord)
    if Config.ALLOW_BLANKS = false then runGame((GameLogic.removeElementFromList(' ', correctAnswer)), []) else 
        runGame (correctAnswer, [])
        

    //printfn "%A xd2" correctAnswer  
    

