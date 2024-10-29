module Tests

open System
open Xunit
open FsCheck
open FsCheck.Xunit

open bt_bag


let tree =
    Empty |> insert 5 |> insert 3 |> insert 7 |> insert 3 |> insert 6 |> insert 8


[<Fact>]
let ``Insert should add elements correctly`` () =
    let result = countElement 3 tree
    Assert.Equal(2, result)

[<Fact>]
let ``Remove should decrease count`` () =
    let updatedTree = remove 3 tree
    Assert.Equal(1, countElement 3 updatedTree)

[<Fact>]
let ``Remove should remove element if count is 1`` () =
    let updatedTree = remove 3 tree |> remove 3
    Assert.Equal(0, countElement 3 updatedTree)


[<Fact>]
let ``Remove root element should restructure tree correctly`` () =
    let updatedTree = remove 5 tree
    Assert.Equal(0, countElement 5 updatedTree)
    Assert.Equal(2, countElement 3 updatedTree)
    Assert.Equal(1, countElement 6 updatedTree)

[<Fact>]
let ``Remove non-existent element should not change tree`` () =
    let updatedTree = remove 4 tree
    Assert.Equal(2, countElement 3 updatedTree)
    Assert.Equal(1, countElement 7 updatedTree)

[<Fact>]
let ``Merge trees with overlapping elements`` () =
    let tree1 = Empty |> insert 5 |> insert 3 |> insert 7
    let tree2 = Empty |> insert 7 |> insert 8 |> insert 5
    let mergedTree = mergeTrees tree1 tree2
    Assert.Equal(2, countElement 5 mergedTree)
    Assert.Equal(2, countElement 7 mergedTree)
    Assert.Equal(1, countElement 3 mergedTree)
    Assert.Equal(1, countElement 8 mergedTree)

[<Fact>]
let ``Complex tree removal and merge`` () =
    let tree1 = Empty |> insert 10 |> insert 5 |> insert 15 |> insert 10
    let tree2 = Empty |> insert 20 |> insert 15 |> insert 25
    let updatedTree1 = remove 10 tree1
    let mergedTree = mergeTrees updatedTree1 tree2
    Assert.Equal(1, countElement 10 mergedTree)
    Assert.Equal(2, countElement 15 mergedTree)
    Assert.Equal(1, countElement 20 mergedTree)
    Assert.Equal(1, countElement 25 mergedTree)

[<Fact>]
let ``Removing all occurrences of an element`` () =
    let updatedTree = remove 3 (remove 3 tree)
    Assert.Equal(0, countElement 3 updatedTree)
    Assert.Equal(1, countElement 5 updatedTree)
    Assert.Equal(1, countElement 6 updatedTree)
    Assert.Equal(1, countElement 8 updatedTree)



let tree2 = Empty |> insert 5 |> insert 5 |> insert 3 |> insert 7 |> insert 6 |> insert 8

[<Fact>]
let ``Fold should sum all elements correctly`` () =
    let sum = foldLeft (+) 0 tree2
    Assert.Equal((10 + 3 + 7 + 6 + 8), sum)

[<Fact>]
let ``Filter should return tree with elements greater than 4`` () =
    let filteredTree = filter (fun value -> value > 4) tree2
    let sum = foldLeft (+) 0 filteredTree
    Assert.Equal((10 + 7 + 6 + 8), sum)

[<Fact>]
let ``Compare trees should return true for identical trees`` () =
    let tree1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let tree2 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))

    let result = compareTrees tree1 tree2
    Assert.True(result)

[<Fact>]
let ``Compare trees should return false for different values`` () =
    let tree1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let tree2 = 
        BNode(Node(5, 2), 
            BNode(Node(4, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))

    let result = compareTrees tree1 tree2
    Assert.False(result)

[<Fact>]
let ``Compare trees should return false for different structure`` () =
    let tree1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let tree2 = 
        BNode(Node(5, 2), 
            Empty, 
            BNode(Node(7, 1), Empty, Empty))

    let result = compareTrees tree1 tree2
    Assert.False(result)

[<Fact>]
let ``Compare trees should return true for two empty trees`` () =
    let tree1 = Empty
    let tree2 = Empty

    let result = compareTrees tree1 tree2
    Assert.True(result)

[<Fact>]
let ``Compare trees should return false for one empty tree`` () =
    let tree1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let tree2 = Empty

    let result = compareTrees tree1 tree2
    Assert.False(result)