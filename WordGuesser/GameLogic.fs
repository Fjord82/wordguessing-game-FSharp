module WordGuesser.GameLogic

open System

// Extras - HELP fuunction
let rec santasLittleHelper (guessedValues : char list, correctAnswer : char list) =
     match correctAnswer with
        |[] -> Char.MinValue
        |h::t when Seq.exists((=) h ) guessedValues -> santasLittleHelper (guessedValues, t)
        |h::t when not (Seq.exists((=) h) guessedValues) -> h 
        |_ -> failwith "Out of Luck, You are on the naugthy list"

let rec removeElementFromList(element, list: char list) = 
     match list with
        |[] -> []
        |h::t when h.Equals(element) -> removeElementFromList(element, t)
        |h::t when not (h.Equals(element)) -> [h] @ removeElementFromList(element, t)
        |_ -> failwith "Something went wrong"


// 1. Get a random word from a list of words - module config (the word to be guessed)
let getRandomWord =
    let words = Config.WORDS
    let random = Random()

    let indexNum = random.Next(words.Length)
    let hiddenWord = if Config.CASE_SENSITIVE then words.[indexNum] else words.[indexNum].ToLower()
    hiddenWord
    //if Config.ALLOW_BLANKS = false then hiddenWord.Replace(' ', Char.MinValue) else
    //    hiddenWord

// 2. write hidden word with "*" markers together with the char that has been guessed
let rec writePartialWord (hiddenWord : char list, guessedValue : char list)  =
    match hiddenWord with
        |[] -> []
        |h::t when h.Equals(' ') && Config.ALLOW_BLANKS -> [' '] @ writePartialWord (t, guessedValue)
        |h::t when Seq.exists(fun elem -> elem.Equals(h)) guessedValue -> [h] @ writePartialWord (t, guessedValue) 
        |h::t when not (Seq.exists(fun elem -> elem.Equals(h)) guessedValue) -> [Config.HIDDEN] @ writePartialWord (t, guessedValue)
        |_ -> failwith "Incorrect key pressed"

// 3. User input, record/read key input from user
let keyIsValid (guessedValue: char list, keyPressed:char) =
    let isValid = not (Seq.exists((=) keyPressed) guessedValue) && (Seq.exists((=) keyPressed) (Seq.append['A'..'Z'] ['a'..'z']))//Seq.append['A'..'Z'] ['a'..'z'])
    isValid

let rec getKeyPressed (guessedValue : char list, correctAnswer : char list) =
    let mutable keyValue = Console.ReadKey(true).KeyChar
    if keyValue.Equals('1') then santasLittleHelper(guessedValue, correctAnswer) else
        let keyValue = if (Config.CASE_SENSITIVE = false) then Char.ToLower keyValue else keyValue
        let isValid = keyIsValid (guessedValue, keyValue)
        if isValid then keyValue else getKeyPressed (guessedValue, correctAnswer)

// 4. Check if input from user is correct or incorrect 
let isGuessCorrect (keypressed : char, answer : char list) =
    Seq.exists ((=) keypressed) answer


