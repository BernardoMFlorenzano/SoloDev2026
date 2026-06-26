# State Machine System / Sistema de Máquina de Estados

### Sistema de Máquina de Estados desenvolvido no projeto Solo Dev 2026, pelo membro Bernardo Miguel Florenzano do projeto Dev-U.

## Estrutura do pacote:
O Sistema de Máquina de Estados se baseia em dois scripts: 
* **MaquinaEstados**: Definição da Classe abstrata MaquinaEstados (Será a classe pai para a implementação de objeto que utilizarão uma Máquina de Estados);
* **EstadoBase**: Definição da Classe abstrata EstadoBase (Será a classe pai para os diferentes estados que uma implementação de Máquina de Estados terá).

## Como usar:
A ideia do pacote é possibilitar o uso de Máquina de Estados para diferentes contextos (Player com diferentes estados, Inimigos com comportamentos complexos etc.).
Na pasta "Exemplos" há um exemplo de implementação de Máquina de Estados, sendo que para criar um nova implementação e criar os estados deve-se seguir esses passos (Recomendado acessar o exemplo para melhor entendimento do processo):

1. Criar uma nova classe com base na classe pai "MaquinaEstados":

`public class NomeDaClasse : MaquinaEstados<NomeDaClasse>`

2. Implementar as características especificas da nova classe (Se necessário, pode se dar override nas funções base de "MaquinaEstados")

3. Definir os estados que pertencem a essa nova Máquina de Estados:

`public InimigoIdle EstadoIdle { get; private set; }`

`public InimigoPatrol EstadoPatrol { get; private set; }`

`...`

4. Na implementação de cada estado, para o acesso de caractéristicas da máquina de estados deve se usar a chamada do contexto atual "ctx", como por exemplo: `ctx.Variavel` ou `ctx.Funcao()`.

5. Utilize override para cada método que será implementado em cada estado (Cada uma dessas ações serão chamadas pela Máquina de Estados):

`public override void EntraEstado()`
`public override void UpdateEstado()`
`public override void FixedUpdateEstado()`
`public override void SaiDeEstado()`
`public override void ChecaTransicoes()`

6. Ao criar novos estados, lembre sempre de defini-los na classe de implementação da Máquina de estados e inicializar-los em "Awake()".


