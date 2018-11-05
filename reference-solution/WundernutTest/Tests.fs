module WundernutTest
open System
open NUnit.Framework
open Wundernut

[<TestFixture>]
type TestClass () = 

    [<Test>]
    member this.``getNeighbors returns 0-4 neighbors based on input size and index`` () =
        let neighbors: int -> string = "abcdefg" |> getNeighbors
        Assert.AreEqual("bc", neighbors 0)
        Assert.AreEqual("acd", neighbors 1)
        Assert.AreEqual("abde", neighbors 2)
        Assert.AreEqual("bcef", neighbors 3)
        Assert.AreEqual("deg", neighbors 5)
        Assert.AreEqual("ef", neighbors 6)

    [<Test>]
    member this.``getNextLine returns string list with next line based and adds up to 3 padding`` () =
        CollectionAssert.AreEqual([".#.##.."; "..##.#."], getNextLine [".#.##.."])
        CollectionAssert.AreEqual([".#.###.."; "..#.###."],  getNextLine [".#.###.."])
        CollectionAssert.AreEqual([".#.#."; "..#.."], getNextLine [".#.#."])
        CollectionAssert.AreEqual(["..#.."; "....."], getNextLine ["..#.."])

    [<Test>]
    member this.``addPadding pads each line until all have at least 3 dots before and after the pattern`` () =
        CollectionAssert.AreEqual(["...foo..."; "...bar..."], addPadding ["foo"; "bar"])
        CollectionAssert.AreEqual(["...foo......"; "......bar..."], addPadding ["foo..."; "...bar"])
        CollectionAssert.AreEqual(["...baz...."], addPadding ["baz."])
        CollectionAssert.AreEqual(["...."], addPadding ["...."])
        CollectionAssert.AreEqual(["...#.##...."], addPadding [".#.##.."])
        CollectionAssert.AreEqual(["...#.###...."],  addPadding [".#.###.."])
        CollectionAssert.AreEqual(["...#.#..."], addPadding [".#.#."])
        CollectionAssert.AreEqual(["...#..."], addPadding ["..#.."])

    [<Test>]
    member this.``getType returns correct type for rows`` () =
        Assert.AreEqual("blinking", getType ["foo"; "bar"; "baz"; "bar"])
        Assert.AreEqual("blinking", getType [".#.##.."; "..##.#."; ".#.##.."])
        Assert.AreEqual("gliding", getType ["foobar"; "oobarf"])
        Assert.AreEqual("gliding", getType [".#.###.."; "..#.###."])
        Assert.AreEqual("vanishing", getType ["foobar"; "..."])
        Assert.AreEqual("vanishing", getType [".#.#."; "..#.."; "....."])
        Assert.AreEqual("other", getType ["foo"; "bar"; "baz"])
        Assert.AreEqual("other", getType ["###"; "..."; "#.#."])

    [<Test>]
    member this.``processPattern finds correct pattern or "other" after 100 iterations`` () =
        Assert.AreEqual("blinking", processPattern [".#.##.."])    
        Assert.AreEqual("blinking", processPattern [".....##...#.###########......."])    
        Assert.AreEqual("gliding", processPattern [".#.###.."])    
        Assert.AreEqual("gliding", processPattern ["....#######.##.##.#.#..."])    
        Assert.AreEqual("vanishing", processPattern [".#.#."])    
        Assert.AreEqual("vanishing", processPattern ["......######......"])    
        Assert.AreEqual("other", processPattern ["#######.##.##.#.#....#.######"])    
        Assert.AreEqual("other", processPattern ["###.#....#.###"])    