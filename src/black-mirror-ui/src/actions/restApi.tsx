const WEB_API = 'https://localhost:8644/'; // process.env.__WEB_API__;
class RestApi {

    get(url: string) {
        // Return a new promise.
        return new Promise(function (resolve: any, reject: any) {
            // Do the usual XHR stuff
            var req = new XMLHttpRequest();
            req.withCredentials = true;
            req.open('GET', url);

            req.onload = function () {
                // This is called even on 404 etc
                // so check the status
                if (req.status === 200) {
                    // Resolve the promise with the response text
                    resolve(req.response);
                } else {
                    // Otherwise reject with the status text
                    // which will hopefully be a meaningful error
                    reject(Error(req.statusText));
                }
            };

            // Handle network errors
            req.onerror = function (e: ErrorEvent) {
                reject(Error(e.message));
            };

            // Make the request
            req.send();
        });
    }

    change(method: string, url: string, body: any) {
        // Return a new promise.
        return new Promise(function (resolve: any, reject: any) {
            // Do the usual XHR stuff
            var req = new XMLHttpRequest();
            req.withCredentials = true;
            req.open(method, url);
            req.setRequestHeader('Content-type', 'application/json');

            req.onload = function () {
                // This is called even on 404 etc
                // so check the status
                if (req.status === 200) {
                    // Resolve the promise with the response text
                    resolve(req.response);
                } else {
                    // Otherwise reject with the status text
                    // which will hopefully be a meaningful error
                    reject(Error(req.statusText));
                }
            };

            // Handle network errors
            req.onerror = function (e: ErrorEvent) {
                reject(Error(e.message));
            };

            // Make the request
            if (body != null) {
                req.send(JSON.stringify(body));
            } else {
                req.send();
            }

        });
    }

    getUserSelf() {
        var url = WEB_API + 'user/self/';

        return this.get(url).then(JSON.parse);
    }

    getUsers() {
        var url = WEB_API + 'user/';

        return this.get(url).then(JSON.parse);
    }

    getUser(id: string) {
        var url = WEB_API + 'user/' + id;

        return this.get(url).then(JSON.parse);
    }

    postUser(user: any) {
        var url = WEB_API + 'user/';

        return this.change('POST', url, user).then(JSON.parse);
    }

    putUser(id: string, user: any) {
        var url = WEB_API + 'user/' + id;

        return this.change('PUT', url, user);
    }

    deleteUser(id: string) {
        var url = WEB_API + 'user/' + id;

        return this.change('DELETE', url, null);
    }

    getMirrors() {
        var url = WEB_API + 'mirror/';

        return this.get(url).then(JSON.parse);
    }

    getMirror(id: string) {
        var url = WEB_API + 'mirror/' + id;

        return this.get(url).then(JSON.parse);
    }

    postMirror(mirror: any) {
        var url = WEB_API + 'mirror/';
        return this.change('POST', url, mirror).then(JSON.parse);
    }

    putMirror(id: string, mirror: any) {
        var url = WEB_API + 'mirror/' + id;
        return this.change('PUT', url, mirror);
    }

    deleteMirror(id: string) {
        var url = WEB_API + 'mirror/' + id;
        return this.change('DELETE', url, null);
    }

    getRepositories() {
        var url = WEB_API + 'repository/';

        return this.get(url).then(JSON.parse);
    }

    getRepository(id: string) {
        var url = WEB_API + 'repository/' + id;

        return this.get(url).then(JSON.parse);
    }

    postRepository(repo: any) {
        var url = WEB_API + 'repository/';
        return this.change('POST', url, repo).then(JSON.parse);
    }

    putRepository(id: string, repository: any) {
        var url = WEB_API + 'repository/' + id;
        return this.change('PUT', url, repository);
    }

    deleteRepository(id: string) {
        var url = WEB_API + 'repository/' + id;
        return this.change('DELETE', url, null);
    }

    getSync(syncId: string) {
        var url = WEB_API + 'sync/' + syncId;

        return this.get(url).then(JSON.parse);
    }

    postSync(mirrorId: any) {
        var url = WEB_API + 'sync/';
        return this.change('POST', url, { MirrorId: mirrorId }).then(JSON.parse);
    }

    getLastSync(mirrorId: string) {
        var url = WEB_API + 'mirror/' + mirrorId + '/sync';

        return this.get(url).then(JSON.parse);
    }

    getSyncLogs(syncId: string) {
        var url = WEB_API + 'sync/' + syncId + '/logs';

        return this.get(url).then(JSON.parse);
    }

    getReflections(syncId: string) {
        var url = WEB_API + 'sync/' + syncId + '/reflections';

        return this.get(url).then(JSON.parse);
    }
}

export default new RestApi();