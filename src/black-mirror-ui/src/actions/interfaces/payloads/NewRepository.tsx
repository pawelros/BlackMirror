export default interface NewRepository {
    Type: string;
    Name: string;
    Uri: string;
    DefaultCommitMessagePrefix: string;
    CheckoutUserId: string;
    PushUserId: string;
    status: string;
    error: any;
    save(payload: any): void;
    refresh(): void;
}