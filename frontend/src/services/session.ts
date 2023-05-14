import { DI } from '@microsoft/fast-foundation';

export interface Session {
    setCurrentUsername(username: string): void;
    username: string;
}

export class SessionImpl implements Session {
    username: string;
    
    setCurrentUsername(username: string): void {
        this.username = username;
    }
}

export const Session = DI.createInterface<Session>(
    x => x.singleton(SessionImpl)
);