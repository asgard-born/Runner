$${\textsf{\color{violet}Project with Reactive-based architecture with single entry point}}$$
• Two-way communication between Pms and Views via reactive data-bindings
• Changes within states cause triggers for business logic to handle it
• Changes within Pms are also change Views and vice versa
• There are no links between Pms and View – they communicates through reactive events
• They also have no public methods. Declarative writing logic
We can think of the global state as a set of reactive states in appropriate entities. When the states are changed - it triggers business logic to handle it.
Entities: composition tree branches, that creates Pms (Presentation Models), Views and connections between them
Pms: business logic
Views: separated view logic without business logic
Ctx: Context – Dependency Injections, which are easy-to-change