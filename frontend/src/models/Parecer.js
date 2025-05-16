export default class Parecer{
    constructor (obj){
        obj = obj || {}
        this.Id = obj.Id
        this.DescriçãoParecer = obj.DescriçãoParecer || '',
        this.UsuarioId = obj.UsuarioId,
        this.DataCadastro = obj.DataCadastro,
        this.AtendimentoId = obj.AtendimentoId
    }
}