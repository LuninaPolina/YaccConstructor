﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;

namespace Highlighting.Core
{
    public class AbstractTreeNode : IAbstractTreeNode
    {
        public ITreeNode Parent { get; private set; }
        public ITreeNode FirstChild { get; private set; }
        public ITreeNode LastChild { get; private set; }
        public ITreeNode NextSibling { get; private set; }
        public ITreeNode PrevSibling { get; private set; }
        public NodeType NodeType { get; private set; }
        public PsiLanguageType Language { get; private set; }
        public NodeUserData UserData { get; private set; }
        public NodeUserData PersistentUserData { get; private set; }
        //public int Parts { get; private set; }
        
        //private DocumentRange documentRange = new DocumentRange();
        private List<DocumentRange> ranges = new List<DocumentRange>();
        private int curInd;

        private string text;

        public AbstractTreeNode(string s)
        {
            text = s;
        }

        public AbstractTreeNode(string s, object positions)
        {
            text = s;
            SetPositions(positions);
        }

        //public virtual void SetDocument(IDocument document)
        //{
        //    documentRange = new DocumentRange(document, 0);
        //}

        //public virtual void SetDocumentRange(DocumentRange range)
        //{
        //    documentRange = range;
        //}

        public void SetPositions(object obj)
        {
            var positions = obj as IEnumerable<DocumentRange>;
            if (positions != null)
            {
                ranges = positions.ToList();
                //UserData.PutData(new Key<List<DocumentRange>>("ranges"), ranges);
            }
        }

        //public virtual DocumentRange[] GetAllPositions()
        //{
        //    return ranges.ToArray();
        //}

        public virtual IPsiServices GetPsiServices()
        {
            return default(IPsiServices);
        }

        public virtual IPsiModule GetPsiModule()
        {
            return Parent.GetPsiModule();
        }

        public virtual IPsiSourceFile GetSourceFile()
        {
            return Parent.GetSourceFile();
        }

        public virtual ReferenceCollection GetFirstClassReferences()
        {
            return default(ReferenceCollection);
        }

        public virtual void ProcessDescendantsForResolve(IRecursiveElementProcessor processor)
        {
            return;
        }

        public virtual T GetContainingNode<T>(bool returnThis = false) where T : ITreeNode
        {
            return default(T);
        }

        public virtual bool Contains(ITreeNode other)
        {
            return true;
        }

        public virtual bool IsPhysical()
        {
            return true;
        }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual bool IsStub()
        {
            return false;
        }

        public virtual bool IsFiltered()
        {
            return true;
        }

        public virtual DocumentRange GetNavigationRange()
        {
            if (ranges.Count == 0)
                return default(DocumentRange);
            if (curInd >= ranges.Count)
                curInd = 0;
            return ranges[curInd++];
        }

        public virtual TreeOffset GetTreeStartOffset()
        {
            return new TreeOffset();
        }

        public virtual int GetTextLength()
        {
            return text.Length;
        }

        public virtual StringBuilder GetText(StringBuilder to)
        {
            for (ITreeNode nextSibling = this.FirstChild; nextSibling != null; nextSibling = nextSibling.NextSibling)
            {
                nextSibling.GetText(to);
            }
            return to;
        }

        public virtual IBuffer GetTextAsBuffer()
        {
            return new StringBuffer(text);
        }

        public virtual string GetText()
        {
            //StringBuilder to = (this.MyCachedLength >= 0) ? new StringBuilder(this.myCachedLength) : new StringBuilder();
            //return this.GetText(to).ToString();
            return text;
        }

        public virtual ITreeNode FindNodeAt(TreeTextRange treeTextRange)
        {
            return null;
        }

        public virtual ICollection<ITreeNode> FindNodesAt(TreeOffset treeTextOffset)
        {
            return default(ICollection<ITreeNode>);
        }

        public virtual ITreeNode FindTokenAt(TreeOffset treeTextOffset)
        {
            return null;
        }

        public virtual void SetParent(ITreeNode parent)
        {
            Parent = parent;
        }

        public virtual void SetFirstChild(ITreeNode firstChild)
        {
            FirstChild = firstChild;
        }

        public virtual void SetLastChild(ITreeNode lastChild)
        {
            LastChild = lastChild;
        }

        public DocumentRange[] GetAllPositions()
        {
            return ranges.ToArray();
        }

        public virtual void SetNextSibling(ITreeNode nextSibling)
        {
            NextSibling = nextSibling;
        }

        public virtual void SetPrevSibling(ITreeNode prevSibling)
        {
            PrevSibling = prevSibling;
        }

        public virtual void SetPsiLanguageType(PsiLanguageType languageType)
        {
            Language = languageType;
        }

        public virtual void SetNodeUserData(NodeUserData userData)
        {
            UserData = userData;
        }

        public virtual void SetPersistentUserData(NodeUserData persistentUserData)
        {
            PersistentUserData = persistentUserData;
        }

        public virtual void Accept(TreeNodeVisitor visitor)
        {
            visitor.VisitSomething(this);
        }

        public virtual void Accept<TContext>(TreeNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.VisitSomething(this, context);
        }

        public virtual TResult Accept<TContext, TResult>(TreeNodeVisitor<TContext, TResult> visitor, TContext context)
        {
            visitor.VisitSomething(this, context);
            return default(TResult);
        }
    }
}
