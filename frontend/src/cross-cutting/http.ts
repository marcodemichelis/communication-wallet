import { DI, inject } from '@microsoft/fast-foundation';
import { Serializer } from './serializer';

export interface Http {
  post<T = any>(url: string, request: any): Promise<T>;
  get<T = any>(url: string): Promise<T>;
  delete<T = any>(url: string): Promise<T>;
}

export interface HttpError {
  title: string;
  status: number;
  detail: string;
  errors: [];
  validationMessage: string;
}
class HttpImpl implements Http {
//  @inject(MessageHandler) messageHandler!: MessageHandler;

  constructor(@Serializer private serializer: Serializer) {}

  async post<T>(url: string, request: any): Promise<T> {
    return await this.call(url, "POST", request);
  }

  async get<T>(url: string): Promise<T> {
    return await this.call(url, "GET", undefined);
  }

  async delete<T>(url: string): Promise<T> {
    return await this.call(url, "DELETE", undefined);
  }

  private async call<T>(url: string, verb: string, request: any): Promise<T> {
    let response;
    response = await fetch(`${url}`, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      method: verb,
      body: request ? JSON.stringify(request) : request,
    });
    await this.handleErrorMessage(response);
    return this.serializer.deserialize<T>(response);
  }

  async handleErrorMessage(httpResponse: any){
    if (!httpResponse.ok)
    {
      const httpError: HttpError = await this.serializer.deserialize<HttpError>(httpResponse);
      const messageText = httpError.validationMessage ? httpError.validationMessage.replace(new RegExp("<br>", 'g'), "\n") : httpError.detail;
//      this.messageHandler.sendMessageRequest({ messageType: "Error", messageText });
      throw new Error("Error to perform call to cstool api endpoints");
    }
  }
}

export const Http = DI.createInterface<Http>(
  x => x.singleton(HttpImpl)
);