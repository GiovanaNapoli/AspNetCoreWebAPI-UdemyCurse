import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';

import { Aluno } from '../model/aluno';

@Injectable({
  providedIn: 'root'
})
export class AlunoService {

  baseURL = `${environment.mainUrlAPI}aluno`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(this.baseURL);
  }

  getById(id: number): Observable<Aluno> {
    return this.http.get<Aluno>(`${this.baseURL}/${id}`);
  }

  getByDisciplinaId(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${this.baseURL}/ByDisciplina/${id}`);
  }

  post(aluno: Aluno) {
    return this.http.post(this.baseURL, aluno);
  }

  put(aluno: Aluno) {
    return this.http.put(`${this.baseURL}/${aluno.id}`, aluno);
  }

  patch(aluno: Aluno) {
    return this.http.patch(`${this.baseURL}/${aluno.id}`, aluno);
  }
  trocarEstado(alunoId: number, ativo: boolean) {
    return this.http.patch(`${this.baseURL}/${alunoId}/trocarEstado`, {estado : ativo});
  }

  delete(id: number) {
    return this.http.delete(`${this.baseURL}/${id}`);
  }
}