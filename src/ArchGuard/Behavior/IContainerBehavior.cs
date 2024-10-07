// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.ArchGuard.BuildingBlock;
using YADA.ArchGuard.Feedback;

namespace YADA.ArchGuard.Behavior
{
    /*

    # container behavior

    defines how a container influences its elements

    ## Layer
    Layer any child has a position
    any access only to lower children
    question: should we support children with the same position, Meaning on the same layer for all other children but
    isolated within this layer so simple isolated 

    ### Idea simple layer 
    box with children which may be placed with one of three position upper, lower and middle. Upper access other in the 
    same container but may not be accessed and lower may be accessed but may not access by others 
    middle means both. An alternative would be proposal above with 3 predefined layers.

    ## Isolated
    isolated, no child may access each other, but this does not apply to any other dependency
    free no restriction at all simple container like a layer

    ## Open
        No restriction at all any child may be access each other

    ## Facade

    only access to a specific child, is this doable with public/private child behavior?
    all other is restricted.

    # Child behavior

    describes the behavior of a single element

    ## public 
        accessible from outside 

    ## private
        not accessible from outside, implementation assembly only within the own parent accessible

    ## protected
        not accessible from outside, only within by sibling building blocks in the same parent, like private
        but it may be as well accessed by extensions


    ## extension

        Idea: an extension may use implements relations. Maybe it may interfere with private could mean there is a protected building block which is accessible from an extension
         the extension can access other subsystem. But regarding cycles it counts as member of the extended subsystem (fold technique)

     */



    /*
     *
     Building Block type as addition next to the container and inner behavior

     In this case any building block type can be used to determine the architectural pattern and
     based in this pattern rules can be activated to check of the usage is correct

    For example:

        Infrastructure

        Domain infrastructure

        Foundation

        Functional

        Output Generation

    All these types have different responsibilities and also own rights. The Access rights form a layer in this case
    But it is not restricted to layers.

    Question does this form an own complete rule set hierarchy? 
    In this case we have a project with several root objects and we have to check any

    Or is it possible to support this with an own rule set and only tags at the entities?

    Are the building blocks the same?
    What is with concrete building blocks? The abstract once like layers are not reusable
    Also it could be that an abstract building block forms a architectural pattern (Infra fro example)

    It could be that it is needed to create concrete building blocks on there own and map them the 
    the different chains.
    
    The BuildingBlock is primary for the chain. Maybe the Description is enough to share.
    But concrete BuildingBlocks maybe need to store there assigned types in this case an own data structure
    should be introduced.



*/
    public interface IContainerBehavior
    {
        void AddBehavior(string containerBehavior);
        bool Check(IReadOnlyList<IBuildingBlock> chain, int currentIndex, IBuildingBlock buildingBlock, ICheckFeedback feedback);
    }
}

