import Mirror from './Mirror';
import SynchronizationStatus from './SynchronizationStatus';

export default interface Synchronization {
    Id: string;
    Mirror: Mirror;
    CreationTime: string;
    Status: SynchronizationStatus;
}