using System;

namespace SQLGeneration.Builders
{
    /// <summary>
    /// Provides a base class for visitors that navigate the builder objects.
    /// </summary>
    public class BuilderVisitor
    {
        /// <summary>
        /// Visits the given builder.
        /// </summary>
        /// <param name="item">The builder to visit.</param>
        public void Visit(IVisitableBuilder item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Accept(this);
        }

        /// <summary>
        /// Visits an Addition builder.
        /// </summary>
        /// <param name="item">The visited Addition builder.</param>
        public virtual void VisitAddition(Addition item)
        {
        }

        /// <summary>
        /// Visits an AllColumns builder.
        /// </summary>
        /// <param name="item">The visited AllColumns builder.</param>
        public virtual void VisitAllColumns(AllColumns item)
        {
        }

        /// <summary>
        /// Visits a BetweenFilter builder.
        /// </summary>
        /// <param name="item">The visited BetweenFilter builder.</param>
        public virtual void VisitBetweenFilter(BetweenFilter item)
        {
        }

        /// <summary>
        /// Visits a BetweenWindowsFrame builder.
        /// </summary>
        /// <param name="item">The visited BetweenWindowFrame builder.</param>
        public virtual void VisitBetweenWindowFrame(BetweenWindowFrame item)
        {
        }

        /// <summary>
        /// Visits a BoundFrame builder.
        /// </summary>
        /// <param name="item">The visited BoundFrame builder.</param>
        public virtual void VisitBoundFrame(BoundFrame item)
        {
        }

        /// <summary>
        /// Visits a Column builder.
        /// </summary>
        /// <param name="item">The visited Column builder.</param>
        public virtual void VisitColumn(Column item)
        {
        }

        /// <summary>
        /// Visits a ConditionalCase builder.
        /// </summary>
        /// <param name="item">The visited ConditionalCase builder.</param>
        public virtual void VisitConditionalCase(ConditionalCase item)
        {
        }

        /// <summary>
        /// Visits a CrossJoin builder.
        /// </summary>
        /// <param name="item">The visited CrossJoin builder.</param>
        public virtual void VisitCrossJoin(CrossJoin item)
        {
        }

        /// <summary>
        /// Visits a CurrentRowFrame builder.
        /// </summary>
        /// <param name="item">The visited CurrentRowFrame builder.</param>
        public virtual void VisitCurrentRowFrame(CurrentRowFrame item)
        {
        }

        /// <summary>
        /// Visits a Delete builder.
        /// </summary>
        /// <param name="item">The visited Delete builder.</param>
        public virtual void VisitDelete(DeleteBuilder item)
        {
        }

        /// <summary>
        /// Visits a Division builder.
        /// </summary>
        /// <param name="item">The visited Division builder.</param>
        public virtual void VisitDivision(Division item)
        {
        }

        /// <summary>
        /// Visits an EqualToFilter builder.
        /// </summary>
        /// <param name="item">The visited EqualToFilter builder.</param>
        public virtual void VisitEqualToFilter(EqualToFilter item)
        {
        }

        /// <summary>
        /// Visits an EqualToQuantifierFilter builder.
        /// </summary>
        /// <param name="item">The visited EqualToQuantifierFilter builder.</param>
        public virtual void VisitEqualToQuantifierFilter(EqualToQuantifierFilter item)
        {
        }

        /// <summary>
        /// Visits an Except builder.
        /// </summary>
        /// <param name="item">The visited Except builder.</param>
        public virtual void VisitExcept(Except item)
        {
        }

        /// <summary>
        /// Visits an ExistsFilter builder.
        /// </summary>
        /// <param name="item">The visited ExistsFilter builder.</param>
        public virtual void VisitExistsFilter(ExistsFilter item)
        {
        }

        /// <summary>
        /// Visits a FilterGroup builder.
        /// </summary>
        /// <param name="item">The visited FilterGroup builder.</param>
        public virtual void VisitFilterGroup(FilterGroup item)
        {
        }

        /// <summary>
        /// Visits a FullOuterJoin builder.
        /// </summary>
        /// <param name="item">The visited FullOuterJoin builder.</param>
        public virtual void VisitFullOuterJoin(FullOuterJoin item)
        {
        }

        /// <summary>
        /// Visits a Function builder.
        /// </summary>
        /// <param name="item">The visited Function builder.</param>
        public virtual void VisitFunction(Function item)
        {
        }

        /// <summary>
        /// Visits a FunctionWindow builder.
        /// </summary>
        /// <param name="item">The visited FunctionWindow builder.</param>
        public virtual void VisitFunctionWindow(FunctionWindow item)
        {
        }

        /// <summary>
        /// Visits a GreaterThanEqualToFilter builder.
        /// </summary>
        /// <param name="item">The visited GreaterThanEqualToFilter builder.</param>
        public virtual void VisitGreaterThanEqualToFilter(GreaterThanEqualToFilter item)
        {
        }

        /// <summary>
        /// Visits a GreaterThanEqualToQuantifierFilter builder.
        /// </summary>
        /// <param name="item">The visited GreaterThanEqualToQuantifierFilter builder.</param>
        public virtual void VisitGreaterThanEqualToQuantifierFilter(GreaterThanEqualToQuantifierFilter item)
        {
        }

        /// <summary>
        /// Visits a GreaterThanFilter builder.
        /// </summary>
        /// <param name="item">The visited GreaterThanFilter builder.</param>
        public virtual void VisitGreaterThanFilter(GreaterThanFilter item)
        {
        }

        /// <summary>
        /// Visits a GreaterThanQuantifierFilter builder.
        /// </summary>
        /// <param name="item">The visited GreaterThanQuantifierFilter builder.</param>
        public virtual void VisitGreaterThanQuantifierFilter(GreaterThanQuantifierFilter item)
        {
        }

        /// <summary>
        /// Visits an InFilter builder.
        /// </summary>
        /// <param name="item">The visited InFilter builder.</param>
        public virtual void VisitInFilter(InFilter item)
        {
        }

        /// <summary>
        /// Visits an InnerJoin builder.
        /// </summary>
        /// <param name="item">The visited InnerJoin builder.</param>
        public virtual void VisitInnerJoin(InnerJoin item)
        {
        }

        /// <summary>
        /// Visits an Insert builder.
        /// </summary>
        /// <param name="item">The visited Insert builder.</param>
        public virtual void VisitInsert(InsertBuilder item)
        {
        }

        /// <summary>
        /// Visits an Intersect builder.
        /// </summary>
        /// <param name="item">The visited Intersect builder.</param>
        public virtual void VisitIntersect(Intersect item)
        {
        }

        /// <summary>
        /// Visits a LeftOuterJoin builder.
        /// </summary>
        /// <param name="item">The visited LeftOuterJoin builder.</param>
        public virtual void VisitLeftOuterJoin(LeftOuterJoin item)
        {
        }

        /// <summary>
        /// Visits a LessThanEqualToFilter builder.
        /// </summary>
        /// <param name="item">The visited LessThanEqualToFilter builder.</param>
        public virtual void VisitLessThanEqualToFilter(LessThanEqualToFilter item)
        {
        }

        /// <summary>
        /// Visits a LessThanEqualToQuantifierFilter builder.
        /// </summary>
        /// <param name="item">Visits a LessThanEqualToQuantifier builder.</param>
        public virtual void VisitLessThanEqualToQuantifierFilter(LessThanEqualToQuantifierFilter item)
        {
        }

        /// <summary>
        /// Visits a LessThanFilter builder.
        /// </summary>
        /// <param name="item">Visits a LessThanFilter builder.</param>
        public virtual void VisitLessThanFilter(LessThanFilter item)
        {
        }

        /// <summary>
        /// Visits a LessThanQuantifierFilter builder.
        /// </summary>
        /// <param name="item">Visits a LessThanQuantifierFilter builder.</param>
        public virtual void VisitLessThanQuantifierFilter(LessThanQuantifierFilter item)
        {
        }

        /// <summary>
        /// Visits a LikeFilter builder.
        /// </summary>
        /// <param name="item">Visits a LikeFilter builder.</param>
        public virtual void VisitLikeFilter(LikeFilter item)
        {
        }

        /// <summary>
        /// Visits a MatchCase builder.
        /// </summary>
        /// <param name="item">Visits a MatchCase builder.</param>
        public virtual void VisitMatchCase(MatchCase item)
        {
        }

        /// <summary>
        /// Visits a Minus builder.
        /// </summary>
        /// <param name="item">Visits a Minus builder.</param>
        public virtual void VisitMinus(Minus item)
        {
        }

        /// <summary>
        /// Visits a Modulus builder.
        /// </summary>
        /// <param name="item">Visits a Modulus builder.</param>
        public virtual void VisitModulus(Modulus item)
        {
        }

        /// <summary>
        /// Visits a Multiplication builder.
        /// </summary>
        /// <param name="item">Visits a Multiplication builder.</param>
        public virtual void VisitMultiplication(Multiplication item)
        {
        }

        /// <summary>
        /// Visits a Negation builder.
        /// </summary>
        /// <param name="item">Visits a Negation builder.</param>
        public virtual void VisitNegation(Negation item)
        {
        }

        /// <summary>
        /// Visits a NotEqualToFilter builder.
        /// </summary>
        /// <param name="item">Visits a NotEqualToFilter builder.</param>
        public virtual void VisitNotEqualToFilter(NotEqualToFilter item)
        {
        }

        /// <summary>
        /// Visits a NotEqualToQuantifierFilter builder.
        /// </summary>
        /// <param name="item">Visits a NotEqualToQuantifierFilter builder.</param>
        public virtual void VisitNotEqualToQuantifierFilter(NotEqualToQuantifierFilter item)
        {
        }

        /// <summary>
        /// Visits a NotFilter builder.
        /// </summary>
        /// <param name="item">Visits a NotFilter builder.</param>
        public virtual void VisitNotFilter(NotFilter item)
        {
        }

        /// <summary>
        /// Visits a NullFilter builder.
        /// </summary>
        /// <param name="item">Visits a NullFilter builder.</param>
        public virtual void VisitNullFilter(NullFilter item)
        {
        }

        /// <summary>
        /// Visits a NullLiteral builder.
        /// </summary>
        /// <param name="item">Visits a NullLiteral builder.</param>
        public virtual void VisitNullLiterator(NullLiteral item)
        {
        }

        /// <summary>
        /// Visits a NumericLiteral builder.
        /// </summary>
        /// <param name="item">Visits a NumericLiteral builder.</param>
        public virtual void VisitNumericLiteral(NumericLiteral item)
        {
        }

        /// <summary>
        /// Visits a OrderBy builder.
        /// </summary>
        /// <param name="item">Visits a OrderBy builder.</param>
        public virtual void VisitOrderBy(OrderBy item)
        {
        }

        /// <summary>
        /// Visits a Placeholder builder.
        /// </summary>
        /// <param name="item">Visits a Placeholder builder.</param>
        public virtual void VisitPlaceholder(Placeholder item)
        {
        }

        /// <summary>
        /// Visits a PreceedingOnlyWindowFrame builder.
        /// </summary>
        /// <param name="item">Visits a PreceedingOnlyWindowFrame builder.</param>
        public virtual void VisitPreceedingOnlyWindowFrame(PrecedingOnlyWindowFrame item)
        {
        }

        /// <summary>
        /// Visits a RightOuterJoin builder.
        /// </summary>
        /// <param name="item">Visits a RightOuterJoin builder.</param>
        public virtual void VisitRightOuterJoin(RightOuterJoin item)
        {
        }

        /// <summary>
        /// Visits a SelectBuilder builder.
        /// </summary>
        /// <param name="item">Visits a SelectBuilder builder.</param>
        public virtual void VisitSelectBuilder(SelectBuilder item)
        {
        }

        /// <summary>
        /// Visits a Setter builder.
        /// </summary>
        /// <param name="item">Visits a Setter builder.</param>
        public virtual void VisitSetter(Setter item)
        {
        }

        /// <summary>
        /// Visits a StringLiteral builder.
        /// </summary>
        /// <param name="item">Visits a StringLiteral builder.</param>
        public virtual void VisitStringLiteral(StringLiteral item)
        {
        }

        /// <summary>
        /// Visits a Subtraction builder.
        /// </summary>
        /// <param name="item">Visits a Subtraction builder.</param>
        public virtual void VisitSubtraction(Subtraction item)
        {
        }

        /// <summary>
        /// Visits a Table builder.
        /// </summary>
        /// <param name="item">Visits a Table builder.</param>
        public virtual void VisitTable(Table item)
        {
        }

        /// <summary>
        /// Visits a Top builder.
        /// </summary>
        /// <param name="item">Visits a Top builder.</param>
        public virtual void VisitTop(Top item)
        {
        }

        /// <summary>
        /// Visits a UnboundFrame builder.
        /// </summary>
        /// <param name="item">Visits a UnboundFrame builder.</param>
        public virtual void VisitUnboundFrame(UnboundFrame item)
        {
        }

        /// <summary>
        /// Visits a Union builder.
        /// </summary>
        /// <param name="item">Visits a Union builder.</param>
        public virtual void VisitUnion(Union item)
        {
        }

        /// <summary>
        /// Visits a Update builder.
        /// </summary>
        /// <param name="item">Visits a Update builder.</param>
        public virtual void VisitUpdate(UpdateBuilder item)
        {
        }

        /// <summary>
        /// Visits a ValueList builder.
        /// </summary>
        /// <param name="item">Visits a ValueList builder.</param>
        public virtual void VisitValueList(ValueList item)
        {
        }
    }
}
