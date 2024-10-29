module PropertyTests
open Xunit
open FsCheck
open FsCheck.Xunit

open bt_bag

type ArrayArbitrary() =
    static member Arb =
        Arb.fromGen (Gen.choose (50, 100) |>  Gen.map (fun length -> Gen.arrayOfLength length (Gen.choose (-50, 50))))


[<Property(Arbitrary = [| typeof<ArrayArbitrary> |])>]
let prop_emptyMerge (n: int[]) =
    let newBag = Array.fold (fun acc x -> insert x acc) Empty n
    compare newBag (merge newBag Empty)



[<Property(Arbitrary = [| typeof<ArrayArbitrary> |])>]
let prop_associativity (n1: int[]) (n2: int[]) =
    let bag1 = Array.fold (fun acc x -> insert x acc) Empty n1
    let bag2 = Array.fold (fun acc x -> insert x acc) Empty n2
    let mergedBag = merge bag1 bag2
    let mergedBag2 = merge bag2 bag1
    compare mergedBag mergedBag2


[<Property(Arbitrary = [| typeof<ArrayArbitrary> |])>]
let prop_compare_non_equal (n1: int[]) (n2: int[]) =
    let bag1 = Array.fold (fun acc x -> insert x acc) Empty n1
    let bag2 = Array.fold (fun acc x -> insert x acc) Empty n2
    n1 = n2 || not (compare bag1 bag2)