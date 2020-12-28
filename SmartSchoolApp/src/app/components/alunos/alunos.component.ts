import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Aluno } from 'src/app/model/aluno';
import { Professor } from 'src/app/model/professor';
import { AlunoService } from 'src/app/services/aluno.service';
import { ProfessorService } from 'src/app/services/professor.service';
import { PaginatedResult, Pagination } from 'src/app/model/Pagination';

@Component({
  selector: 'app-alunos',
  templateUrl: './alunos.component.html',
  styleUrls: ['./alunos.component.scss'],
})
export class AlunosComponent implements OnInit, OnDestroy {

  public modalRef: BsModalRef;
  public alunoForm: FormGroup;
  public titulo = 'Alunos';
  public alunoSelecionado: Aluno;
  public textSimple: string;
  public profsAlunos: Professor[];
  public alunos: Aluno[];
  public aluno: Aluno;
  public modeSave = 'post';
  public msnDeleteAluno: string;
  pagination: Pagination;

  private unsubscriber = new Subject();

  constructor(
    private alunoService: AlunoService,
    private route: ActivatedRoute,
    private professorService: ProfessorService,
    private fb: FormBuilder,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.criarForm();
  }

  ngOnInit() {
    this.pagination = {currentPage: 1, itemsPerPage: 4} as Pagination;
    this.carregarAlunos();
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  professoresAlunos(template: TemplateRef<any>, id: number) {
    this.spinner.show();
    this.professorService
      .getByAlunoId(id)
      .pipe(takeUntil(this.unsubscriber))
      .subscribe(
        (professores: Professor[]) => {
          this.profsAlunos = professores;
          this.modalRef = this.modalService.show(template);
        },
        (error: any) => {
          this.toastr.error(`erro: ${error}`);
          console.error(`Erro: ${error.message}`);
          this.spinner.hide();
        },
        () => this.spinner.hide()
      );
  }

  criarForm() {
    this.alunoForm = this.fb.group({
      id: [0],
      nome: ['', Validators.required],
      sobrenome: ['', Validators.required],
      telefone: ['', Validators.required],
      ativo: []
    });
  }

  saveAluno() {
    if (this.alunoForm.valid) {
      this.spinner.show();

      if (this.modeSave === 'post') {
        this.aluno = { ...this.alunoForm.value };
      } else {
        this.aluno = { id: this.alunoSelecionado.id, ...this.alunoForm.value };
      }

      this.alunoService[this.modeSave](this.aluno)
        .pipe(takeUntil(this.unsubscriber))
        .subscribe(
          () => {
            this.carregarAlunos();
            this.toastr.success('Aluno salvo com sucesso!');
          },
          (error: any) => {
            this.toastr.error(`Erro: Aluno não pode ser salvo!`);
            console.error(`Erro: ${error.message}`);
            this.spinner.hide();
          },
          () => this.spinner.hide()
        );
    }
  }

  trocarEstado(aluno: Aluno){
    this.alunoService.trocarEstado(aluno.id, !aluno.ativo)
        .pipe(takeUntil(this.unsubscriber))
        .subscribe(
          (resp) => {
            this.carregarAlunos();
            this.toastr.success('Aluno salvo com sucesso!');
          },
          (error: any) => {
            this.toastr.error(`Erro: Aluno não pode ser salvo!`);
            console.error(`Erro: ${error.message}`);
            this.spinner.hide();
          },
          () => this.spinner.hide()
        );
  }

  carregarAlunos(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.spinner.show();
    this.alunoService
      .getAll(this.pagination.currentPage, this.pagination.itemsPerPage)
      .pipe(takeUntil(this.unsubscriber))
      .subscribe(
        (alunos: PaginatedResult<Aluno[]>) => {
          this.alunos = alunos.result;
          this.pagination = alunos.pagination;

          if (id > 0) {
            this.alunoSelect(id);
          }

          this.toastr.success('Alunos foram carregado com Sucesso!');
        },
        (error: any) => {
          this.toastr.error("Alunos não podem ser carregados");
          console.error(`Erro: ${error.message}`);
          this.spinner.hide();
        },
        () => this.spinner.hide()
      );
  }

  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.carregarAlunos();
  }

  alunoSelect(alunoId: number): void {
    this.modeSave = 'patch';
    this.alunoService.getById(alunoId).subscribe(
      (alunoReturn) => {
        this.alunoSelecionado = alunoReturn;
        this.toastr.success('Alunos foram carregado com Sucesso!');
        this.alunoForm.patchValue(this.alunoSelecionado);
      },
      (error) => {
        this.toastr.error("Alunos não podem ser carregados");
        console.error(`Erro: ${error.message}`);
        this.spinner.hide();
      },
      () => this.spinner.hide()
    );
  }

  voltar() {
    this.alunoSelecionado = null;
  }

  openModal(template: TemplateRef<any>, alunoId: number) {
    this.professoresAlunos(template, alunoId);
  }

  closeModal() {
    this.modalRef.hide();
  }
}
