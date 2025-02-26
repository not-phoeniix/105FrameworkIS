# Documentation!

A docs directory for general notes, brainstorm, and other non-code ideas/files we want backed up by git :]

# Basic Inclusions

##### Curriculum "Order"

>>This is the jist of a structure for how to organize topics, I am no professor but this logically makes sense to me - Joe
   - Coding Environment
   - Data
        > Text
        > Numerics
        > Expressions
    
    - Statements
        > Assignment
        > If/Else If/Else
        > While/Do While
        > For/For Each
        > Switch
        > Possibly some other variant like T ? T : F
    
    - Lists/Arrays
        > Multi-Dim
        > Dictionaries

    - Functions/Methods
        > Parameters
        > Recursion
        > More parameters (ie param, out, in, ref, etc..) 

    - Classes
        > Abstraction
        > Polymorphism
        > Inheritance
        > Structs
        > Enums
        > Namespace
        > Scope
    
##### Ideas

We could create a series of psuedo-code docs like you would normally get in 105/106 that guides the student
to get a specific outcome in the exercises that is screenshotted to be submitted. For example, *if* it was 
a factory setting, and you load into it without typing any of your own code, you'd be able to interact normally (whatever that is)
and in the code if you were to spawn 3 boxes, just making them make a function that uses our own SpawnBox() to 
loop through would be a good example

First exercise, give a name to a box, it shows up in game, spawn a couple
Second exercise, they make an array of names, and loop through the names and spawn boxes with each name showcasing loops and Arrays

Something like this imo, still needs to be a little wrung in considering the topics themselves would be a little out of order in my examples.


Toy Box
// Everything is an entity, collision with each other and bounds of window
- Ball
- [X] Box 
- [ ] Character
    - Controllable with {WASD/Arrows}
- [ ] Rope
    - Can be attached to entities
- [X] Conveyor 
    - When in collision applies force
- [X] Force Field
    - Rect region that applies force when inside of
- [ ] Fan
    - Box that blows/sucks things in the direction facing
- [ ] Physics UI Element
    - UI that interacts with other physics components, ie, dragging with verlet

Engine/Toys/Entities