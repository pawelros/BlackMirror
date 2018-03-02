import { observable } from 'mobx';
import RestApi from './restApi';
import Payloads from './interfaces/payloads/Payloads';

class AdminStore {
    @observable currentUser = {
        Id: '',
        Name: '',
        Email: '',
        Exists: false
    };

    @observable users = [{ Id: '', Name: '' }];

    @observable repositories = [];

    @observable mirrors = [];

    @observable payloads: Payloads = {
        newUser: {
            Id: '',
            Name: '',
            Email: '',
            repositoryCredentials: {
                newLogin: '',
                newType: 'svn',
                newPassword: '',
                // tslint:disable-next-line:no-any
                existing: [],
                add: function () {
                    this.existing.push({
                        Login: this.newLogin,
                        Password: this.newPassword,
                        Type: this.newType
                    });
                    this.newLogin = '';
                    this.newType = 'svn';
                    this.newPassword = '';
                },
                // tslint:disable-next-line:no-any
                delete: function (c: any) {
                    delete this.existing[this.existing.indexOf(c)];
                }
            },
            status: 'created',
            save: function () {
                let self = this;
                let request = {
                    Id: this.Id,
                    Name: this.Name,
                    Email: this.Email,
                    // tslint:disable-next-line:no-any
                    RepositoryCredentials: this.repositoryCredentials.existing.map(function (c: any) {
                        return {
                            Login: c.Login,
                            Password: c.Password,
                            RepositoryType: c.Type,
                            AllowedRepositories: [
                                'ALL'
                            ]
                        };
                    })
                };

                // tslint:disable-next-line:no-any
                RestApi.postUser(request).then(function (response: any) {
                    self.status = 'success';
                }, function () {
                    self.status = 'failed';
                });
            }
        },
        newRepository: {
            Type: 'svn',
            Name: '',
            Uri: '',
            DefaultCommitMessagePrefix: '',
            CheckoutUserId: '',
            PushUserId: '',
            status: 'created',
            error: '',
            // tslint:disable-next-line:no-any
            save: function (payload: any) {
                let self = payload;
                let request = {
                    Type: payload.Type,
                    Name: payload.Name,
                    Uri: payload.Uri,
                    DefaultCommitMessagePrefix: payload.DefaultCommitMessagePrefix,
                    CheckoutUserId: payload.CheckoutUserId,
                    PushUserId: payload.PushUserId
                };

                // tslint:disable-next-line:no-any
                RestApi.postRepository(request).then(function (response: any) {
                    self.status = 'success';
                    // tslint:disable-next-line:no-any
                }, function (errgh: any) {
                    self.status = 'failed';
                    self.error = errgh;
                }).then(function () {
                    self.refresh();
                });
            },
            refresh: this.loadRepositories
        },
        newMirror: {
            Name: '',
            SourceRepositoryId: '',
            TargetRepositoryId: '',
            status: 'created',
            error: '',
            // tslint:disable-next-line:no-any
            save: function (payload: any) {
                let self = payload;
                let request = {
                    Name: payload.Name,
                    SourceRepositoryId: payload.SourceRepositoryId,
                    TargetRepositoryId: payload.TargetRepositoryId
                };

                // tslint:disable-next-line:no-any
                RestApi.postMirror(request).then(function (response: any) {
                    self.status = 'success';
                    // tslint:disable-next-line:no-any
                }, function (errgh: any) {
                    self.status = 'failed';
                    self.error = errgh;
                });
            }
        }
    };

    loadUsers() {
        RestApi.getUsers().then((response) => {
            this.users = response;
            // tslint:disable-next-line:no-any
        }, function (error: any) {
            // tslint:disable-next-line:no-console
            console.error('Failed2!', error);
        });
    }

    loadRepositories() {
        RestApi.getRepositories().then((response) => {
            // tslint:disable-next-line:no-any
            response = response.map((item: any) => {
                return Object.assign({ Description: '[' + item.Type + '] ' + item.Name }, item);
            });
            this.repositories = response;
            // tslint:disable-next-line:no-any
        }, function (error: any) {
            // tslint:disable-next-line:no-console
            console.error('Failed2!', error);
        });
    }

    constructor() {
        RestApi.getUserSelf().then((response) => {

            if (response.Exists) {
                this.currentUser = response.Identity;
            } else {
                this.currentUser = response.Auth;
            }

            this.currentUser.Exists = response.Exists;

            if (!this.currentUser.Exists) {
                this.payloads.newUser.Id = this.currentUser.Id;
                this.payloads.newUser.Name = this.currentUser.Name.replace('.', ' ')
                    .replace(/\b\w/g, l => l.toUpperCase());
                this.payloads.newUser.Email = this.currentUser.Name + '@your-email.com';
            }
        }, function (error: any) {
            // tslint:disable-next-line:no-console
            console.error('Failed2!', error);
        });

        this.loadRepositories();

        RestApi.getMirrors().then((response) => {

            this.mirrors = response;
        }, function (error: any) {
            // tslint:disable-next-line:no-console
            console.error('Failed2!', error);
        });

        this.loadUsers();
    }
}

export default new AdminStore();
