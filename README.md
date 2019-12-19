# CrystalClear
The Crystal Clear Engine is a highly WIP game and software engine, so far written entierly in C#.

# Goals
One of the main focuses of the engine is to make the development process as easy and elegant as possible. This means minimal boilerplate code, many utility methods, rapid prototyping and powerful debugging tools*. The engine's code is also heavily documented and openly available to peek at, and of course contribute to!

**to be implemented.*

# The engine

*World*

The world in Crystal Clear is built up of HierarchyObjects. These magnificent beings live in Hierarchies, massive trees which can be quickly traversed, and that can be loaded and unloaded (even saved to be loaded from their previous state!) at will.


*Scripting*

The brains of your game or software is scripts. HierarchyScripts are attatchable to HierarchyObjects, although all scripting does not have to be done in them. Static methods can be subscribed to events, and can work just as well.

The HierarchyScripting way of scripting is commonly known as component-based scripting, although this is not entierly accurate to how scripting works in Crystal Clear.


*Customizability*

Not only can you create your own scripts, nearly all facets of the engine are expandable. You can create custom HierarchyObject types, in fact, the default HierarchyObject types are all manually implementable (anything in `CrystalClear.Standard` you can create yourself!). Custom events can be defined and used as well.

Creating custom is really easy.


*Platform support*

Currently *no* platforms are supported, since exporting has not been implemented yet. The editor is supported on Windows, but nowhere else at the moment. This will likely change, especially as cross-platform options are explored. One likely direction for the engine is to use Mono, which will massively increase the platform support.


# Try it out
Since the engine is not even nearly complete at this point, there is no version of the engine available to download. Sorry about that :/

# The future
The engine will be considered to have entered alpha when the following features have all been made functional:

1. ScriptingEngine

2. EventSystem

3. HierarchySystem

4. UI

5. RenderEngine

6. RuntimeMain
