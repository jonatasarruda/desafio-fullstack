export default class Cliente{
    constructor (obj){
        obj = obj || {}
        this.Id = obj.Id
        this.Nome = obj.Nome || '',
        this.DataCadastro = obj.DataCadastro,
        this.Atendimentos = obj.Atendimentos
    }
}