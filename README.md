# AHpx.RG

Another readme generator for .NET.

## Indroduction

As you can see, this is a readme generator, it simply do some chores for you.

Usually you'll give some member/type a xml document like `<summary>...</summary>`, the tool will parse it and combine it with reflection stuff, so you don't have to write an API reference manually.

Feel free to open an [issue](https://github.com/SinoAHpx/AHpx.RG/issues) or make an [pull request](https://github.com/SinoAHpx/AHpx.RG/pulls).

<details>
    <summary>Why it named this?</summary>
    <p>I don't really know how to name this project, but this is my first attempt at building a useful <a href="https://github.com/AvaloniaUI/Avalonia">Avalonia</a> application, a  name suchlike "ReadmeGenerator" is too unremarkble, so I name this project "AHpx.RG".</p>
    <ul>
        <li><code>AHpx</code>: me, myself and I.</li>
        <li><code>RG</code>: abbreviation for "readme generator.</li>
    </ul>
</details>

### Supported list

+ Type
    + Classes
    + Interfaces
    + Enums
    + Structs
    + Delegates
+ Members
    + Constructors
    + Methods
    + Fields
    + Properties
    + Events

### Sample

```markdown

### TestLib1

This is test1, basic

#### Constructors

+ Constructor1: this is a constructor
  + ```p1(string)```
  + ```p2(Dicitonatry[int, string])```

#### Methods

+ ```Test1(void)```
  + ```p1(string)```: this is a test parameter 1

#### Fields

+ ```TestField1(string)```: tp4

#### Properties

+ ```TestProperty3(string)```: tp3

### TestLib4

this is test lib4

+ Remark: this is a remark
+ ```T```: this is class-level type arg
```

## Start

Even though AHpx.RG is a very easy to use tool, but there's an user guide for you.

### Installation

+ Download your OS-corresponded binary from [latest release](https://github.com/SinoAHpx/AHpx.RG/releases).
+ Extract it to any directory you prefer.
+ Find the file named "AHpx.RG" and run it as executable.

### Build your project

As I told you before, AHpx.RG is based on reflection and xml summary documentation, so you have to toggled the "Generate XML Documentation" option in the project properties.

#### Visual Studio

![image](https://user-images.githubusercontent.com/34391004/154978742-6b836f99-57f4-4d79-9a0b-7a816f21aa6b.png)

#### Jetbrains Rider

![image](https://user-images.githubusercontent.com/34391004/154978886-cb9f40d4-8290-4af1-9cd3-70dca7b689cd.png)

### Next step

When you do as above, you'll see a window like this(it will be automatically turn to dark mode if there's night time). There's 3 text fields in first expander, `Compile dll path` and `Xml documentation path` is required, `Repository tree link` will be used for header of each type if it has also been filled.

Apprently, `Compile dll path` is the path of the dll you want to generate readme for, and `Xml documentation path` is the path of the xml documentation file corresponded.

![image](https://user-images.githubusercontent.com/34391004/154976416-cd7c5f9a-99dd-4e3b-9c19-e87f8191690a.png)


Once you fill the text fields, `Types` and `Previewer` expanders will be visible, and you can choose which types you'd like to generator a readme for by checking the checkboxes. Once you choose, the readme will be generated and shown in `Previewer` instantly.

![image](https://user-images.githubusercontent.com/34391004/154977599-65a61ff8-966d-4255-b7cf-7f5756329c0e.png)

You can expand the `Previewer` to see the generated readme, and you can also copy the generated readme to clipboard, that's it, your readme is ready.

### Remarks

If you got an exception which said "xx file not found", try to add the dependencies in `Dependency libraries` expander, becasue the RG use an `AssemblyLoadContext` internally, this will load dependencies into this context.

## Credits

Without them, this project wouldn't be possible:

### IDE

+ [Jetbrains](https://jetbrains.com/): provided free license

### Libraries

+ [Avalonia](https://avaloniaui.net/): you can see the GUI because of it
+ [ReactiveUI](https://reactiveui.net/): a great library for MVVM
+ [Material.Avalonia](https://github.com/AvaloniaCommunity/Material.Avalonia): brings RG a gorgeous look
+ [Markdown.Avalonia](https://github.com/whistyun/Markdown.Avalonia): that's why you can have an instant preview
+ [MessageBox.Avalonia](https://github.com/AvaloniaCommunity/MessageBox.Avalonia): if you didn't see the message box in RG, you are lucky guy that never got an exception
+ [Manganese](https://github.com/SinoAHpx/Manganese): AHpx's self-made tool library

### People

+ Myself: [SinoAHpx](https://avatars.githubusercontent.com/u/34391004?s=120&v=4)
