import NewUser from './NewUser';
import NewMirror from './NewMirror';
//import EditMirror from './EditMirror';
import NewRepository from './NewRepository';

export default interface Payloads {
    newUser: NewUser;
    newRepository: NewRepository;
    newMirror: NewMirror;
  //  editMirror: EditMirror;
}