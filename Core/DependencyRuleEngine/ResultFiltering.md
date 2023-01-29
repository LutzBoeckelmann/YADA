# Filter the result

In a realistic scenario it is not always possible to have a 100% clean code base.
This situation is supported by base line feature. The feedback of the rule engine
may be filtered and only not filtered feedback will be further processed. 

## How Feedback works

An important aspect of **YADA** is the way how feedback is collected. The idea
is, surely mentioned before, that only with an understandable  and appropriate
feedback message an architectural rule is useful. Therefore for any violation
should add feedback.

The way how the engine works leads to a natural order of feedback messages.
At the first level a **TypeFeedback** will be added. At this type feedback instance
any rule may add it own **RuleFeedback**, which provide the possibility to add
**Information** and the **DependencyContext** for any dependency which violates
the rule.

Any feedback will be added in a general typed way. To provide an understandable
and appropriate feedback a Collector is used which translates the typed feedback
into natural language. To do this any kind of feedback is explorable by a
**FeedbackVisitor**, where it calls the corresponding message with all available
informations. The instance implementing this **FeedbackVisitor** creates the user
feedback. 

One advantage of this decoupling is that the general typed feedback may be 
recorded and filtered. This can be used to base line the current situation
and only detect new issues and violations.

## Feedback recording

To introduce a filter to ignores any older violation, a base line needs to be
created containing any current violations.
Therefore the **FeedbackCollector** will be used and the informations will be
piped into a file. There is an implementation available to dump the current
situation. **FeedbackRecorder** implements **IFeedbackVisitor** and provides a
method **WriteFeedbackResults** to write the collected feedback to given file.
It is important that the feedback will not changed by any other **IFeedbackVisitor**
upfront and the raw information can be dumped.

## Feedback filtering

As corresponding class to the **FeedbackRecorder** the **FeedbackFilter** is
available. This class can read and interpret the base line file.
For sure this filter has to be included in your tests. This is possible because
it acts as a decorator for an **IFeedbackVisitor**. The **FeedbackFilter** accepts
an other **IFeedbackVisitor** as constructor argument.
After a feedback was processed the call is passed to the next **IFeedbackVisitor**.
In this case processing means the **FeedbackFilter** checks if the current violation
is part of the baseline file which means it is already known and accepted. Only
unknown new violations are propagated to the next instance of the **IFeedbackVisitor**.
