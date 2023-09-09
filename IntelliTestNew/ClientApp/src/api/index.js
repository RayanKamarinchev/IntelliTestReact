const host = 'https://localhost:7269/';

async function request(method, url, data){
    const options = {
        method,
        headers: {'Access-Control-Allow-Origin':'*'}
    }

    if(data !== undefined && Object.keys(data).length !== 0){
        options.headers['Content-Type'] = 'application/json'
        options.body = JSON.stringify(data);
    }

    // try {
    //     const response  = await fetch(host + url, options);
    //
    //     if(response.status == 204){
    //         return response;
    //     }
    //     const result = await response.json();
    //
    //     if(response.ok == false){
    //         throw new Error(result.message);
    //     }
    //
    //     return result;
    // } catch (err){
    //     alert(err.message);
    //     throw err;
    // }
    return await fetch(host + url, options);
}

const get = request.bind(null, 'get');
const post = request.bind(null, 'post');
const put = request.bind(null, 'put');
const del = request.bind(null, 'delete');

const api = {
    get,post,put,del
}
export default api