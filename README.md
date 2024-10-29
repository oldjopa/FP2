# FP1
## Лабораторная работа №1 

```Янковой Леонид Кириллович```

```367672```

## Язык - F#

## Вариант - bt-bag

Реализована структура данных Bag на основе бинарного дерева 

Структура данных имеет следующий интерфейс:
```
type BTreeElement<'T> = Node of ('T * int)

type BTreeMultiset<'T when 'T: comparison> =
    | Empty
    | BNode of BTreeElement<'T> * BTreeMultiset<'T> * BTreeMultiset<'T>


insert x bag

remove x bag

countElement x bag

merge bag1 bag2

countElements bag

compare bag1 bag2

```

реализацию можно посмотреть [тут](./lab2/bt-bag.fs)


### Property tests

при помощи кривого порта QuickCheck под названием FsCheck были написаны проперти тесты на ассоциотивность слияния, нейтральный элемент а так же проверка сравнения двух разных мультимножеств
```f#
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

```

### Unit-tasting

```f#

[<Fact>]
let ``Insert should add elements correctly`` () =
    let result = countElement 3 bag
    Assert.Equal(2, result)

[<Fact>]
let ``Remove should decrease count`` () =
    let updatedBag = remove 3 bag
    Assert.Equal(1, countElement 3 updatedBag)

[<Fact>]
let ``Remove should remove element if count is 1`` () =
    let updatedBag = remove 3 bag |> remove 3
    Assert.Equal(0, countElement 3 updatedBag)

[<Fact>]
let ``Remove root element should restructure bag correctly`` () =
    let updatedBag = remove 5 bag
    Assert.Equal(0, countElement 5 updatedBag)
    Assert.Equal(2, countElement 3 updatedBag)
    Assert.Equal(1, countElement 6 updatedBag)

[<Fact>]
let ``Remove non-existent element should not change bag`` () =
    let updatedBag = remove 4 bag
    Assert.Equal(2, countElement 3 updatedBag)
    Assert.Equal(1, countElement 7 updatedBag)

[<Fact>]
let ``Merge bags with overlapping elements`` () =
    let bag1 = Empty |> insert 5 |> insert 3 |> insert 7
    let bag2 = Empty |> insert 7 |> insert 8 |> insert 5
    let mergedBag = merge bag1 bag2
    Assert.Equal(2, countElement 5 mergedBag)
    Assert.Equal(2, countElement 7 mergedBag)
    Assert.Equal(1, countElement 3 mergedBag)
    Assert.Equal(1, countElement 8 mergedBag)

[<Fact>]
let ``Complex bag removal and merge`` () =
    let bag1 = Empty |> insert 10 |> insert 5 |> insert 15 |> insert 10
    let bag2 = Empty |> insert 20 |> insert 15 |> insert 25
    let updatedBag1 = remove 10 bag1
    let mergedBag = merge updatedBag1 bag2
    Assert.Equal(1, countElement 10 mergedBag)
    Assert.Equal(2, countElement 15 mergedBag)
    Assert.Equal(1, countElement 20 mergedBag)
    Assert.Equal(1, countElement 25 mergedBag)

[<Fact>]
let ``Removing all occurrences of an element`` () =
    let updatedBag = remove 3 (remove 3 bag)
    Assert.Equal(0, countElement 3 updatedBag)
    Assert.Equal(1, countElement 5 updatedBag)
    Assert.Equal(1, countElement 6 updatedBag)
    Assert.Equal(1, countElement 8 updatedBag)


let bag2 = Empty |> insert 5 |> insert 5 |> insert 3 |> insert 7 |> insert 6 |> insert 8

[<Fact>]
let ``Fold should sum all elements correctly`` () =
    let sum = foldLeft (+) 0 bag2
    Assert.Equal((10 + 3 + 7 + 6 + 8), sum)


[<Fact>]
let ``Filter should return bag with elements greater than 4`` () =
    let filteredBag = filter (fun value -> value > 4) bag2
    let sum = foldLeft (+) 0 filteredBag
    Assert.Equal((10 + 7 + 6 + 8), sum)


[<Fact>]
let ``Compare bags should return false for different counts`` () =
    let bag1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let bag2 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 2), Empty, Empty))

    let result = compare bag1 bag2
    Assert.False(result)

[<Fact>]
let ``Compare bags should return false for different bags`` () =
    let bag1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let bag2 = 
        BNode(Node(5, 2), 
            Empty, 
            BNode(Node(7, 1), Empty, Empty))

    let result = compare bag1 bag2
    Assert.False(result)

[<Fact>]
let ``Compare bags should return true for two empty bags`` () =
    let bag1 = Empty
    let bag2 = Empty

    let result = compare bag1 bag2
    Assert.True(result)

[<Fact>]
let ``Compare bags should return false for one empty bag`` () =
    let bag1 = 
        BNode(Node(5, 2), 
            BNode(Node(3, 1), Empty, Empty), 
            BNode(Node(7, 1), Empty, Empty))
    
    let bag2 = Empty

    let result = compare bag1 bag2
    Assert.False(result)

```


## Заключение

В ходе выполнения данной лабораторной работы я более подробно изучил деревья и операции с ними, а так же познакомился с концепцией Property-based тестов и в целом начал задаваться вопросом а как же я жил до этого момента, практически не задумываясь о написании тестов к своим программным продуктам