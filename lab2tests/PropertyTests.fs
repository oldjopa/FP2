module PropertyTests

open Xunit
open FsCheck
open FsCheck.Xunit

open bt_bag

type ArrayArbitrary() =
    static member Arb =
        Arb.fromGen (
            Gen.choose (0, 100)
            |> Gen.map (fun length -> Gen.arrayOfLength length (Gen.choose (-50, 50)))
        )

type ArbitraryBTreeMultiset<'T when 'T : comparison>() =
    static member BTree =
        Arb.fromGen <| 
            gen {
                let! size = Gen.choose (10, 50)
                let! values = Gen.listOfLength size (Arb.generate<'T>)
                return List.fold (fun acc n -> insert n acc) Empty values
            }

[<Property(Arbitrary = [| typeof<ArbitraryBTreeMultiset<int>> |])>]
let prop_emptyMerge (bag: BTreeMultiset<int>) =

    compare bag (merge bag Empty)



[<Property(Arbitrary = [| typeof<ArbitraryBTreeMultiset<int>> |])>]
let prop_associativity (bag1: BTreeMultiset<int>) (bag2: BTreeMultiset<int>) (bag3: BTreeMultiset<int>) =
    let mergedBag = merge bag1 (merge bag2 bag3)
    let mergedBag2 = merge (merge bag1 bag2) bag3
    compare mergedBag mergedBag2


[<Property(Arbitrary = [| typeof<ArrayArbitrary> |])>]
let prop_compare_non_equal (n1: int[]) (n2: int[]) =
    let bag1 = Array.fold (fun acc x -> insert x acc) Empty n1
    let bag2 = Array.fold (fun acc x -> insert x acc) Empty n2
    n1 = n2 || not (compare bag1 bag2)
