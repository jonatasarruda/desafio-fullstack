export default class Usuario{
    constructor (obj){
        obj = obj || {}
        this.Id = obj.Id
        this.Email = obj.Email || '',
        this.Token = obj.Token,
        this.ClienteId = obj.ClienteId,
        this.DataCadastro = obj.DataCadastro
    }
}