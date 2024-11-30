## <u> $${\textsf{\color{violet}Project  with  Reactive-based  architecture  with  single  entry  point}}$$ </u>
#### [-> DOCUMENTATION HERE (Ru) <-](https://docs.google.com/document/d/1P_m1-pK7PYw3LxIv4IdjDfgQtoojhUAlO8sDL_k9miw/edit?usp=sharing)
#### • Two-way communication between Pms and Views via reactive data-bindings</br>• Changes within Pm<i>s</i> are also change View<i>s</i> and vice versa</br>• There are no links between Pm<i>s</i> and View – they communicates through reactive events</br>• They also have no public methods. Declarative writing logic

#### Roots: composition tree branches, that creates Pms (Presentation Models), Views and connections between them</br>Pms: business logic</br>Views: separated view logic without business logic</br>Ctx: Context – Dependency Injections, which are easy-to-change
#<img width="600" alt="image_2024-01-16_14-14-07" src="https://github.com/user-attachments/assets/4458447a-b776-47c4-8569-cc487d2cf23a">
