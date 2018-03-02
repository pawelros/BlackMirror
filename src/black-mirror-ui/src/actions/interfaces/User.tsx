import Auth from './Auth';
import Identity from './Identity';

export default interface User {
    Auth: Auth;
    Exists: boolean;
    Identity: Identity;
  }