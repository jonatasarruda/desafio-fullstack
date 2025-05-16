export default class Atendimento{
    constructor (obj){
        obj = obj || {}
        this.Id = obj.Id
        this.TextoAberturaAtendimento = obj.TextoAberturaAtendimento || '',
        this.UsuarioId = obj.UsuarioId,
        this.ClienteId = obj.ClienteId,
        this.DataCadastro = obj.DataCadastro,
        this.Pareceres = obj.Pareceres
    }
}